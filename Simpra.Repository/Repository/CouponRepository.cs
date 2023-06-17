using Simpra.Core.Entity;
using Simpra.Core.Repository;

namespace Simpra.Repository.Repository
{
    public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(AppDbContext context) : base(context)
        {

        }

    }


}
