// Revisado
using API.DbContexts;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.DataServices
{
    public class OrderDetailDataService : BaseDataService<OrderDetail, (int, int)>, IOrderDetailDataService
    {
        public OrderDetailDataService(ApplicationDbContext context) : base(context) { }

        protected override (int, int) GetKey(OrderDetail entity)
        {
            return (entity.OrderId, entity.ProductId);
        }

        public override async Task<OrderDetail> GetByKey((int, int) key)
        {
            return await _context.OrderDetails.FindAsync(key.Item1, key.Item2);
        }

        public override async Task<bool> Exists((int, int) key)
        {
            return await _context.OrderDetails.AnyAsync(od => od.OrderId == key.Item1 && od.ProductId == key.Item2);
        }

        public async Task<List<OrderDetail>> GetOrderDetails(int orderId)
        {
            return await _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .AsNoTracking()
                .ToListAsync();
        }
    }

}