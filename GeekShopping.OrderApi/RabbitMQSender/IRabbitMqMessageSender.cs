using GeekShopping.MessageBus;

namespace GeekShopping.OrderApi.RabbitMQSender {
    public interface IRabbitMqMessageSender {
        void SendMessage(BaseMessage message, string queueName);
    }
}
