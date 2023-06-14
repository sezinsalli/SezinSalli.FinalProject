using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Schema.CategoryRR;
using Simpra.Schema.CouponRR;
using Simpra.Service.Reponse;
using Simpra.Service.Service.Abstract;
using Simpra.Service.Service.Concrete;

namespace Simpra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IService<Coupon> _service;
        private readonly ICouponService _couponService;


        public CouponController(IMapper mapper, IService<Coupon> service, ICouponService couponService)
        {
            _service = service;
            _mapper = mapper;
            _couponService = couponService;

        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var coupons = await _service.GetAllAsync();
            var couponResponse = _mapper.Map<List<CouponResponse>>(coupons.ToList());

            return Ok(CustomResponse<List<CouponResponse>>.Success(200, couponResponse));
        }

        // TODO: mapleme işi kontrollerda yapılacak
        // TODO: bir userın birden fazla koupon kodu olabilir mi?
        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponCreateRequest couponCreateRequest)
        {
            var response = await _couponService.CreateCouponAsync(couponCreateRequest);
            return CreateActionResult(response);
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
