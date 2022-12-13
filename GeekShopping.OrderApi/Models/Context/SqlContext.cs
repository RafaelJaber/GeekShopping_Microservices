using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderApi.Models.Context {
    public class SqlContext : DbContext {
        
        public SqlContext(DbContextOptions<SqlContext> options) : base(options) {}

        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
    }
}
