using AutoMapper;
using GeekShopping.OrderApi.Models;
using GeekShopping.OrderApi.Models.Context;
using GeekShopping.OrderApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderApi.Repository {
    public class OrderRepository : IOrderRepository {
        
        private readonly DbContextOptions<SqlContext> _context;

        public OrderRepository(DbContextOptions<SqlContext> context)
        {
            _context = context;
        }
        
        
        public async Task<bool> AddOrder(OrderHeader header)
        {
            if (header == null) return false;
            await using SqlContext db = new SqlContext(_context);
            db.OrderHeaders.Add(header);
            await db.SaveChangesAsync();
            return true;
        }
        
        public async Task UpdateOrderPaymentStatus(long orderHeaderId, bool paid)
        {
            await using SqlContext db = new SqlContext(_context);
            OrderHeader? header = await db.OrderHeaders.FirstOrDefaultAsync(o => o.Id == orderHeaderId);
            if (header != null){
                header.PaymentStatus = paid;
                await db.SaveChangesAsync();
            }
        }
    }
}
