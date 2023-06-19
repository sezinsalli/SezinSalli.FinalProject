using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Schema.CouponRR;
using Simpra.Service.Response;

namespace Simpra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IBaseService<Coupon> _service;
        private readonly ICouponService _couponService;


        public CouponController(IMapper mapper, IBaseService<Coupon> service, ICouponService couponService)
        {
            _service = service;
            _mapper = mapper;
            _couponService = couponService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var coupons = await _service.GetAllAsync();
            var couponsResponse = _mapper.Map<List<CouponResponse>>(coupons.ToList());
            return CreateActionResult(CustomResponse<List<CouponResponse>>.Success(200, couponsResponse));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponCreateRequest couponCreateRequest)
        {
            var coupon = _mapper.Map<Coupon>(couponCreateRequest);
            var response = await _couponService.CreateCouponAsync(coupon, couponCreateRequest.ExpirationDay);
            var couponResponse = _mapper.Map<CouponResponse>(response);
            return CreateActionResult(CustomResponse<CouponResponse>.Success(200, couponResponse));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var coupon = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(coupon);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }
    }
}
