using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.IdentityServer.Entities.Context {
    public class SqlContext : IdentityDbContext<ApplicationUser> {
        public SqlContext() {}
        public SqlContext(DbContextOptions<SqlContext> options) : base(options) {}
    }
}
