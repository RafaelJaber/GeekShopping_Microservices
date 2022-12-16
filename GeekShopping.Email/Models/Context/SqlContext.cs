using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Models.Context {
    public class SqlContext : DbContext {
        
        
        public SqlContext(DbContextOptions<SqlContext> options) : base(options) {}
        
        public DbSet<EmailLog> EmailLogs { get; set; }

    }
}
