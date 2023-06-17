using Simpra.Core.Entity;
using Simpra.Schema.OrderRR;

namespace Simpra.Core.Service
{
    public interface IOrderService : IBaseService<Order>
    {
        List<Order> GetOrdersWithOrderDetails();
        Task<Order> CreateOrderAsync(Order order);
        void CheckDigitalWalletBalance(ref User user, ref Order order);
        void CheckCouponUsing(ref Coupon coupon, ref Order order);
        decimal EarnPoints(Order order, User user);
        Task<Order> CreateOrderFromMessage(OrderCreateRequest orderRequest);
    }
}
