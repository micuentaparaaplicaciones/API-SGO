// Revisado
using API.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace API.DataServices
{
    public class CustomerDataService : BaseDataService<Customer, int>, ICustomerDataService
    {
        public CustomerDataService(ApplicationDbContext context) : base(context) { }

        protected override int GetKey(Customer entity)
        {
            return entity.Id;
        }

        public override async Task<Customer> GetByKey(int key)
        {
            return await _context.Customers.FindAsync(key);
        }

        public override async Task<bool> Exists(int key)
        {
            return await _context.Customers.AnyAsync(c => c.Id == key);
        }
    }
}