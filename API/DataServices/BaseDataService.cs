// Revisado
using API.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace API.DataServices
{
    public abstract class BaseDataService<T, TKey> : IBaseDataService<T, TKey> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseDataService(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Este método debe devolver la clave de la entidad (simple o compuesta)
        protected abstract TKey GetKey(T entity);

        public virtual async Task<List<T>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<T> GetByKey(TKey key)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(e => GetKey(e).Equals(key));
        }

        public virtual async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Update(T entity)
        {
            if (!await Exists(GetKey(entity)))
                throw new KeyNotFoundException($"Entidad con clave {GetKey(entity)} no encontrada para actualizar.");

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Remove(T entity)
        {
            if (!await Exists(GetKey(entity)))
                throw new KeyNotFoundException($"Entidad con clave {GetKey(entity)} no encontrada para eliminar.");

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task AddMultiple(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateMultiple(List<T> entities)
        {
            foreach (var entity in entities)
            {
                if (!await Exists(GetKey(entity)))
                    throw new KeyNotFoundException($"Entidad con clave {GetKey(entity)} no encontrada para actualizar.");
            }

            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task RemoveMultiple(List<T> entities)
        {
            foreach (var entity in entities)
            {
                if (!await Exists(GetKey(entity)))
                    throw new KeyNotFoundException($"Entidad con clave {GetKey(entity)} no encontrada para eliminar.");
            }

            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> Exists(TKey key)
        {
            return await _dbSet.AnyAsync(e => GetKey(e).Equals(key));
        }
    }
}