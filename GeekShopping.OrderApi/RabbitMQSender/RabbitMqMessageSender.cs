using GeekShopping.OrderApi.Messages;
using GeekShopping.MessageBus;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;

namespace GeekShopping.OrderApi.RabbitMQSender {
    public class RabbitMqMessageSender : IRabbitMqMessageSender {

        private readonly string _hostName;
        private readonly string _passWord;
        private readonly string _userName;
        private IConnection _connection;

        public RabbitMqMessageSender()
        {
            _hostName = "localhost";
            _passWord = "guest";
            _userName = "guest";
        }

        public void SendMessage(BaseMessage message, string queueName)
        {
            if (!ConnectionExists()) return;
            using IModel channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);

            byte[] body = GetMessageAsByteArray(message);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }

        // Usado para serializar a mensagem em bytes
        private static byte[] GetMessageAsByteArray(BaseMessage message)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize<PaymentVo>((PaymentVo)message, options);
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
