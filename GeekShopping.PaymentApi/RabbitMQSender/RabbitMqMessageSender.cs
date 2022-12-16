using GeekShopping.MessageBus;
using GeekShopping.PaymentApi.Messages;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;

namespace GeekShopping.PaymentApi.RabbitMQSender {
    public class RabbitMqMessageSender : IRabbitMqMessageSender {

        private readonly string _hostName;
        private readonly string _passWord;
        private readonly string _userName;
        private IConnection _connection;
        private const string ExchangeName = "FanoutPaymentUpdateExchange";

        public RabbitMqMessageSender()
        {
            _hostName = "localhost";
            _passWord = "guest";
            _userName = "guest";
        }

        public void SendMessage(BaseMessage message)
        {
            if (!ConnectionExists()) return;
            using IModel channel = _connection.CreateModel();
            channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout, durable: false);

            byte[] body = GetMessageAsByteArray(message);
            channel.BasicPublish(exchange: ExchangeName, routingKey: "", basicProperties: null, body: body);
        }

        // Usado para serializar a mensagem em bytes
        private static byte[] GetMessageAsByteArray(BaseMessage message)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize<UpdatePaymentResultMessage>((UpdatePaymentResultMessage)message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }

        private void CreateConnection()
        {
            try{
                ConnectionFactory factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _passWord
                };
                _connection = factory.CreateConnection();  
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
        
        private bool ConnectionExists()
        {
            if (_connection != null) return true;
            CreateConnection();
            return _connection != null;
        }
    }
}
