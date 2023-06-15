using Simpra.Core.Entity;
using Simpra.Schema.CouponRR;
using Simpra.Service.Reponse;

namespace Simpra.Service.Service.Abstract
{
    public interface ICouponService : IService<Coupon>
    {
        Task<CustomResponse<CouponResponse>> CreateCouponAsync(CouponCreateRequest couponCreateRequest);
    }
}
