using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CouponApi.Models.Context {
    public class SqlContext : DbContext {
        
        public SqlContext(DbContextOptions<SqlContext> options) : base(options) {}
        
        public DbSet<Coupon> Coupons { get; set; }
        
    }
}
