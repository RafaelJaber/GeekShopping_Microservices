using GeekShopping.CartAPI.Models.ValueObjects;

namespace GeekShopping.CartAPI.Repository.Interface {
    public interface ICouponRepository {
        Task<CouponVo> GetCouponByCode(string couponCode, string token);
    }
}
