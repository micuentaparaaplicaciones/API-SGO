// Revisado
using API.Models;

namespace API.DataServices
{
    public interface IOrderDetailDataService : IBaseDataService<OrderDetail, (int, int)>
    {
        Task<List<OrderDetail>> GetOrderDetails(int orderId);
    }
}