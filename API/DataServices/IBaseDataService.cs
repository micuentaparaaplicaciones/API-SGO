// Revisado
namespace API.DataServices
{
    public interface IBaseDataService<T, TKey> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetByKey(TKey key);
        Task Add(T entity);
        Task Update(T entity);
        Task Remove(T entity);
        Task AddMultiple(List<T> entities);
        Task UpdateMultiple(List<T> entities);
        Task RemoveMultiple(List<T> entities);
        Task<bool> Exists(TKey key);
    }
}