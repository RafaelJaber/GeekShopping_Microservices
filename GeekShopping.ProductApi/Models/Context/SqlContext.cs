using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductApi.Models.Context {

    public class SqlContext : DbContext {
        public SqlContext() {}
        public SqlContext(DbContextOptions<SqlContext> options) : base(options) {}

        public DbSet<Product> Products { get; set; }

    }


}
