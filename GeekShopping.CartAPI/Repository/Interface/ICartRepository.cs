using GeekShopping.CartAPI.Models.ValueObjects;

namespace GeekShopping.CartAPI.Repository.Interface {
    public interface ICartRepository {
        Task<CartVo> FindCartByUserId(string userId);
        Task<CartVo> SaveOrUpdateCart(CartVo vo);
        Task<bool> RemoveFromCart(long cartDetailsId);
        Task<bool> ApplyCoupon(string userId, string cuponCode);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);
    }
}
