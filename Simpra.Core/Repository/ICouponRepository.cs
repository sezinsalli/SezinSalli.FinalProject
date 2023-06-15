using Simpra.Core.Entity;

namespace Simpra.Core.Repository
{
    public interface ICouponRepository : IGenericRepository<Coupon>
    {
        Task<Coupon> CreateCouponAsync(Coupon coupon);
        bool IsCouponCodeExists(string couponCode);

    }
}
