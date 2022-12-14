using GeekShopping.PaymentApi.Messages;
using GeekShopping.PaymentApi.RabbitMQSender;
using GeekShopping.PaymentProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.PaymentApi.MessageConsumer {
    public class RabbitMqPaymentConsumer : BackgroundService {

        private readonly IModel _channel;
        private IRabbitMqMessageSender _rabbitMqMessageSender;
        private readonly IProcessPayment _processPayment;

        public RabbitMqPaymentConsumer( IProcessPayment processPayment, IRabbitMqMessageSender rabbitMqMessageSender)
        {
            _processPayment = processPayment;
            _rabbitMqMessageSender = rabbitMqMessageSender;
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            IConnection connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(queue: "orderPaymentProcessQueue", false, false, false, arguments: null);

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) => {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                PaymentMessage? vo = JsonSerializer.Deserialize<PaymentMessage>(content);
                ProcessPayment(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("orderPaymentProcessQueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessPayment(PaymentMessage? vo)
        {
            var result = _processPayment.PaymentProcessor();

            UpdatePaymentResultMessage paymentResultMessage = new UpdatePaymentResultMessage()
            {
                Status = result,
                OrderId = vo.OrderId,
                Email = vo.Email
            };
            
            try{
                _rabbitMqMessageSender.SendMessage(paymentResultMessage, "orderPaymentResultQueue");
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
