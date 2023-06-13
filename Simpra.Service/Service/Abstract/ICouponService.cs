using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Schema.CouponRR;
using Simpra.Service.Reponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Service.Service.Abstract
{
    public interface ICouponService : IService<Coupon>
    {
        Task<CustomResponse<CouponResponse>> CreateCouponAsync(CouponCreateRequest couponCreateRequest);
    }
}
