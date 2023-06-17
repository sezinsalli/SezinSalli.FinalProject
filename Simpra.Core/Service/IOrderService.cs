using Simpra.Core.Entity;
using Simpra.Schema.OrderRR;

namespace Simpra.Core.Service
{
    public interface IOrderService : IBaseService<Order>
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> CreateOrderFromMessage(OrderCreateRequest orderRequest);
        Task<Order> UpdateOrderStatusAsync(int id, string status);
    }
}
