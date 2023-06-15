using Simpra.Core.Entity;

namespace Simpra.Service.Service.Abstract
{
    public interface IOrderService : IService<Order>
    {
        List<Order> GetOrdersWithOrderDetails();
        Task<Order> CreateOrderAsync(Order order);
        void CheckDigitalWalletBalance(ref User user, ref Order order);
        void CheckCouponUsing(ref Coupon coupon, ref Order order);
        decimal EarnPoints(Order order, User user);
    }
}
