using Simpra.Core.Entity;

namespace Simpra.Core.Service
{
    public interface IOrderService : IBaseService<Order>
    {
        Task<Order> CreateOrderAsync(Order order, string userId, string hashedCreditCard);
        Task<Order> UpdateOrderStatusAsync(int id, int status, string username);
    }
}
