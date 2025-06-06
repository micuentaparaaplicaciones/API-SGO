// Revisado
using API.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace API.DataServices
{
    public class OrderDataService : BaseDataService<Order, int>, IOrderDataService
    {
        public OrderDataService(ApplicationDbContext context) : base(context) { }

        protected override int GetKey(Order entity)
        {
            return entity.Id;
        }

        public override async Task<Order> GetByKey(int key)
        {
            return await _context.Orders.FindAsync(key);
        }

        public override async Task<bool> Exists(int key)
        {
            return await _context.Orders.AnyAsync(o => o.Id == key);
        }
    }
}