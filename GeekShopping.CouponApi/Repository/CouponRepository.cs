using AutoMapper;
using GeekShopping.CouponApi.Models;
using GeekShopping.CouponApi.Models.Context;
using GeekShopping.CouponApi.Models.ValueObjects;
using GeekShopping.CouponApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CouponApi.Repository {
    public class CouponRepository : ICouponRepository {

        private readonly IMapper _mapper;
        private readonly SqlContext _context;

        public CouponRepository(IMapper mapper, SqlContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CouponVo> GetCouponByCode(string code)
        {
            Coupon? coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponCode == code && c.ExpireDate <= DateTime.Now);
            return _mapper.Map<CouponVo>(coupon);

        }
        public async Task<CouponVo> RegisterNewCoupon(CouponVo vo)
        {
            Coupon coupon = _mapper.Map<Coupon>(vo);
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<CouponVo>(coupon);

        }
        public async Task<bool> DeleteCoupon(long uid)
        {
            Coupon? coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Id == uid);
            if (coupon == null) return false;
            coupon.ExpireDate = DateTime.Now.AddDays(-1);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
