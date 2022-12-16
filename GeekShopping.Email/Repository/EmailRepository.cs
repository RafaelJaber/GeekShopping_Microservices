using GeekShopping.Email.Messages;
using GeekShopping.Email.Models;
using GeekShopping.Email.Models.Context;
using GeekShopping.Email.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repository {
    public class EmailRepository : IEmailRepository {
        
        private readonly DbContextOptions<SqlContext> _context;

        public EmailRepository(DbContextOptions<SqlContext> context)
        {
            _context = context;
        }

        
        public async Task LogEmail(UpdatePaymentResultMessage message)
        {
            EmailLog email = new EmailLog()
            {
                Email = message.Email,
                SentDate = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfully!"
            };
            await using SqlContext db = new SqlContext(_context);
            db.EmailLogs.Add(email);
            await db.SaveChangesAsync();
        }
    }
}
