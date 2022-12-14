using GeekShopping.OrderApi.Messages;
using GeekShopping.OrderApi.Models;
using GeekShopping.OrderApi.RabbitMQSender;
using GeekShopping.OrderApi.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderApi.MessageConsumer {
    public class RabbitMqPaymentConsumer : BackgroundService {

        private readonly OrderRepository _repository;
        private readonly IModel _channel;
        private const string ExchangeName = "FanoutPaymentUpdateExchange";
        private string _queueName = "";

        public RabbitMqPaymentConsumer(OrderRepository repository, IRabbitMqMessageSender rabbitMqMessageSender)
        {
            _repository = repository;
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            IConnection connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queueName, ExchangeName, "");

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) => {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                UpdatePaymentResultVo? vo = JsonSerializer.Deserialize<UpdatePaymentResultVo>(content);
                UpdatePaymentStatus(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume(_queueName, false, consumer);
            return Task.CompletedTask;
        }

        private async Task UpdatePaymentStatus(UpdatePaymentResultVo? vo)
        {
            try{
                if (vo != null) await _repository.UpdateOrderPaymentStatus(vo.OrderId, vo.Status);
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
