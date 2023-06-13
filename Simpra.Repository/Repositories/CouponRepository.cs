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
    public class CouponRepository :  GenericRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(AppDbContext context) : base(context)
        {
            
        }


    }

}
