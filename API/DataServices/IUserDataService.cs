// Revisado
using API.Models;

namespace API.DataServices
{
    public interface IUserDataService : IBaseDataService<User, int>
    {
        Task<User> GetByEmail(string email);
    }
}