using GeekShopping.CouponApi.Models.ValueObjects;

namespace GeekShopping.CouponApi.Repository.IRepository {
    public interface ICouponRepository {
        Task<CouponVo> GetCouponByCode(string code);
        Task<CouponVo> RegisterNewCoupon(CouponVo vo);
        Task<bool> DeleteCoupon(long uid);
    }
}
