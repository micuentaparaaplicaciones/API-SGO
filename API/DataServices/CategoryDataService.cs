// Revisado
using API.DbContexts;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.DataServices
{
    public class CategoryDataService : BaseDataService<Category, int>, ICategoryDataService
    {
        public CategoryDataService(ApplicationDbContext context) : base(context) { }

        protected override int GetKey(Category entity)
        {
            return entity.Id;
        }

        public override async Task<Category> GetByKey(int key)
        {
            return await _context.Categories.FindAsync(key);
        }

        public override async Task<bool> Exists(int key)
        {
            return await _context.Categories.AnyAsync(c => c.Id == key);
        }
    }
}