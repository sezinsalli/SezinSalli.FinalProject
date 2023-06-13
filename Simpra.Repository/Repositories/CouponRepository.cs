using Microsoft.EntityFrameworkCore;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Repository.Repositories
{
    public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(AppDbContext context) : base(context)
        {

        }
        
        public async Task<Coupon> CreateCouponAsync(Coupon coupon)
        {           
            await _appDbContext.Coupons.AddAsync(coupon);
            await _appDbContext.SaveChangesAsync();
            return coupon;
        }

        public bool IsCouponCodeExists(string couponCode)
        {
            return _appDbContext.Coupons.Any(c => c.CouponCode == couponCode);
        }

    }


}
