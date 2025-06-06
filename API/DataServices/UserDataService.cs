// Revisado
using API.DbContexts;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.DataServices
{
    public class UserDataService : BaseDataService<User, int>, IUserDataService
    {
        public UserDataService(ApplicationDbContext context) : base(context) { }

        protected override int GetKey(User entity)
        {
            return entity.Id;
        }

        public override async Task<User> GetByKey(int key)
        {
            return await _context.Users.FindAsync(key);
        }

        public override async Task<bool> Exists(int key)
        {
            return await _context.Users.AnyAsync(u => u.Id == key);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}