using GeekShopping.MessageBus;

namespace GeekShopping.PaymentApi.RabbitMQSender {
    public interface IRabbitMqMessageSender {
        void SendMessage(BaseMessage message, string queueName);
    }
}
