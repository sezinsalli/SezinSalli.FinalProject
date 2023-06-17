using AutoMapper;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.Service;
using Simpra.Core.UnitofWork;
using Simpra.Schema.CouponRR;

namespace Simpra.Service.Service
{
    public class CouponService : BaseService<Coupon>, ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        public CouponService(IGenericRepository<Coupon> repository, IUnitOfWork unitofWork, ICouponRepository couponRepository, IMapper mapper) : base(repository, unitofWork)
        {
            _mapper = mapper;
            _couponRepository = couponRepository;
        }

        public async Task<CouponResponse> CreateCouponAsync(CouponCreateRequest couponCreateRequest)
        {
            string couponCode = GenerateUniqueCouponCode();
            DateTime expirationDate = DateTime.Now.AddDays(couponCreateRequest.ExpirationDay);
            var coupon = new Coupon
            {
                UserId = couponCreateRequest.UserId,
                CouponCode = couponCode,
                DiscountAmount = couponCreateRequest.DiscountAmount,
                ExpirationDate = expirationDate,
                IsActive = true,
            };

            var couponResult = await _couponRepository.CreateCouponAsync(coupon);

            var couponResponse = _mapper.Map<CouponResponse>(couponResult);

            return couponResponse;
        }

        private string GenerateUniqueCouponCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string couponCode;

            do
            {
                couponCode = new string(Enumerable.Repeat(chars, 10)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            while (_couponRepository.IsCouponCodeExists(couponCode));

            return couponCode;
        }


    }
}
