using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.IServices {
    public interface ICouponService {
        Task<CouponViewModel?> GetCouponByCode(string code, string token);
        Task<CouponViewModel> CreateCoupon(CouponViewModel model, string token);
        Task<bool> DeleteCoupon(long id, string token);
    }
}
