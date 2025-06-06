// Revisado
using API.DbContexts;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.DataServices
{
    public class SupplierDataService : BaseDataService<Supplier, int>, ISupplierDataService
    {
        public SupplierDataService(ApplicationDbContext context) : base(context) { }

        protected override int GetKey(Supplier entity)
        {
            return entity.Id;
        }

        public override async Task<Supplier> GetByKey(int key)
        {
            return await _context.Suppliers.FindAsync(key);
        }

        public override async Task<bool> Exists(int key)
        {
            return await _context.Suppliers.AnyAsync(s => s.Id == key);
        }
    }
}