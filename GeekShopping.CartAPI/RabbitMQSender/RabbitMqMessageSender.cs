using GeekShopping.CartAPI.Messages;
using GeekShopping.MessageBus;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;

namespace GeekShopping.CartAPI.RabbitMQSender {
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
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _passWord
            };
            _connection = factory.CreateConnection();

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
            var json = JsonSerializer.Serialize<CheckoutHeaderVo>((CheckoutHeaderVo)message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }
    }
}
