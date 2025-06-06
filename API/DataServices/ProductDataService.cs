// Revisado
using API.DbContexts;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.DataServices
{
    public class ProductDataService : BaseDataService<Product, int>, IProductDataService
    {
        public ProductDataService(ApplicationDbContext context) : base(context) { }

        protected override int GetKey(Product entity)
        {
            return entity.Id;
        }

        public override async Task<Product> GetByKey(int key)
        {
            return await _context.Products.FindAsync(key);
        }

        public override async Task<bool> Exists(int key)
        {
            return await _context.Products.AnyAsync(p => p.Id == key);
        }
    }
}
