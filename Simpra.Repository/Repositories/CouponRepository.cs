using Simpra.Core.Entity;
using Simpra.Core.Repository;

namespace Simpra.Repository.Repositories
{
    public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Coupon> CreateCouponAsync(Coupon coupon)
        {
            await _context.Coupons.AddAsync(coupon);
            await _context.SaveChangesAsync();
            return coupon;
        }

        public bool IsCouponCodeExists(string couponCode)
        {
            return _context.Coupons.Any(c => c.CouponCode == couponCode);
        }

    }


}
