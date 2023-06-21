using Simpra.Core.Entity;
using Simpra.Schema.CreditCardRR;
using Simpra.Schema.OrderRR;

namespace Simpra.Core.Service
{
    public interface IOrderService : IBaseService<Order>
    {
        Task<Order> CreateOrderAsync(Order order, string userId,string hashedCreditCard);
        Task<Order> UpdateOrderStatusAsync(int id, int status);
    }
}
