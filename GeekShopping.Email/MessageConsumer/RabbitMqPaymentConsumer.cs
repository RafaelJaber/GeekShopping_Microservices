using GeekShopping.Email.Messages;
using GeekShopping.Email.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.Email.MessageConsumer {
    public class RabbitMqPaymentConsumer : BackgroundService {

        private readonly EmailRepository _repository;
        private readonly IModel _channel;
        private const string ExchangeName = "FanoutPaymentUpdateExchange";
        private readonly string _queueName;

        public RabbitMqPaymentConsumer(EmailRepository repository)
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
                UpdatePaymentResultMessage? message = JsonSerializer.Deserialize<UpdatePaymentResultMessage>(content);
                ProcessLogs(message).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume(_queueName, false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessLogs(UpdatePaymentResultMessage? message)
        {
            try{
                if (message != null) await _repository.LogEmail(message);
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
