﻿using Simpra.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Core.Repository
{
    public interface ICouponRepository : IGenericRepository<Coupon>
    {
        Task<Coupon> CreateCouponAsync(Coupon coupon);
        bool IsCouponCodeExists(string couponCode);

    }
}
