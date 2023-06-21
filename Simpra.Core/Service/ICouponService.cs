using Simpra.Core.Entity;

namespace Simpra.Core.Service
{
    public interface ICouponService : IBaseService<Coupon>
    {
        Task<Coupon> CreateCouponAsync(Coupon coupon, int expirationDay, string createdBy);
    }
}
