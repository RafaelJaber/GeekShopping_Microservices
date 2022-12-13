using GeekShopping.OrderApi.Models;

namespace GeekShopping.OrderApi.Repository.IRepository {
    public interface IOrderRepository {
        Task<bool> AddOrder(OrderHeader header);
        Task UpdateOrderPaymentStatus(long orderHeaderId, bool paid);
    }
}
