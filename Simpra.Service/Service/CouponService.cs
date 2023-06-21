using Serilog;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.Service;
using Simpra.Core.UnitofWork;

namespace Simpra.Service.Service
{
    public class CouponService : BaseService<Coupon>, ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CouponService(IUnitOfWork unitofWork, ICouponRepository couponRepository) : base(couponRepository, unitofWork)
        {
            _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
            _unitOfWork = unitofWork ?? throw new ArgumentNullException(nameof(unitofWork));
        }

        public async Task<Coupon> CreateCouponAsync(Coupon coupon, int expirationDay, string createdBy)
        {
            try
            {
                var couponCode = await GenerateUniqueCouponCode();
                DateTime expirationDate = DateTime.Now.AddDays(expirationDay);
                var newCoupon = new Coupon
                {
                    UserId = coupon.UserId,
                    CreatedBy = createdBy,
                    CouponCode = couponCode,
                    DiscountAmount = coupon.DiscountAmount,
                    ExpirationDate = expirationDate,
                    IsActive = true,
                };

                await _couponRepository.AddAsync(newCoupon);
                await _unitOfWork.CompleteAsync();
                return newCoupon;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "CreateCouponAsync Exception");
                throw new Exception($"Coupon cannot create. Error message:{ex.Message}");
            }
        }

        private async Task<string> GenerateUniqueCouponCode()
        {
            try
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                string couponCode;

                do
                {
                    couponCode = new string(Enumerable.Repeat(chars, 10)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
                }
                while (await _couponRepository.AnyAsync(x => x.CouponCode == couponCode));
                return couponCode;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GenerateUniqueCouponCode Exception");
                throw new Exception($"Something went wrong! Error message:{ex.Message}");
            }
        }


    }
}
