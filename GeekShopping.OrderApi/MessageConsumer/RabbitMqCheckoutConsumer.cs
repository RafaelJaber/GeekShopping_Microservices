using GeekShopping.OrderApi.Messages;
using GeekShopping.OrderApi.Models;
using GeekShopping.OrderApi.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderApi.MessageConsumer {
    public class RabbitMqCheckoutConsumer : BackgroundService {

        private readonly OrderRepository _repository;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqCheckoutConsumer(OrderRepository repository)
        {
            _repository = repository;
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "checkoutQueue", false, false, false, arguments: null);

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) => {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                CheckoutHeaderVo? vo = JsonSerializer.Deserialize<CheckoutHeaderVo>(content);
                ProcessOrder(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("checkoutQueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessOrder(CheckoutHeaderVo? vo)
        {
            OrderHeader orderHeader = new OrderHeader
            {
                UserId = vo?.UserId,
                FirstName = vo?.FirstName,
                LastName = vo?.LastName,
                OrderDetails = new List<OrderDetail>(),
                CardNumber = vo?.CardNumber,
                CuponCode = vo?.CuponCode,
                DiscountTotal = vo.DiscountTotal,
                Email = vo?.Email,
                ExpiryMothYear = vo?.ExpiryMothYear,
                OrderTime = DateTime.Now,
                PaymentStatus = false,
                Phone = vo?.Phone,
                DateTime = vo.DateTime,
                Cvv = vo.Cvv
            };
            if (vo.CartDetails != null){
                foreach (CartDetailVo item in vo.CartDetails){
                    OrderDetail detail = new()
                    {
                        ProductId = item.ProductId,
                        ProductName = item.Product?.Name,
                        Price = item.Product.Price,
                        Count = item.Count,
                    };
                    orderHeader.CartTotalItems += item.Count;
                    orderHeader.OrderDetails.Add(detail);
                }
            }
            await _repository.AddOrder(orderHeader);
        }
    }
}
