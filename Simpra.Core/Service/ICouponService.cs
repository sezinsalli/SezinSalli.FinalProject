using Simpra.Core.Entity;
using Simpra.Schema.CouponRR;

namespace Simpra.Core.Service
{
    public interface ICouponService : IBaseService<Coupon>
    {
        Task<Coupon> CreateCouponAsync(Coupon coupon, int expirationDay);
    }
}
