using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.UnitofWork;
using Simpra.Repository.Repositories;
using Simpra.Schema.CouponRR;
using Simpra.Schema.ProductwithCategoryRR;
using Simpra.Service.Exceptions;
using Simpra.Service.Reponse;
using Simpra.Service.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Service.Service.Concrete
{
    public class CouponService : Service<Coupon>, ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        public CouponService(IGenericRepository<Coupon> repository, IUnitOfWork unitofWork, ICouponRepository couponRepository, IMapper mapper) : base(repository, unitofWork)
        {
            _mapper = mapper;
            _couponRepository = couponRepository;
        }

        public async Task<CustomResponse<CouponResponse>> CreateCouponAsync(CouponCreateRequest couponCreateRequest)
        {
            string couponCode = GenerateUniqueCouponCode();
            DateTime expirationDate = DateTime.Now.AddDays(couponCreateRequest.ExpirationDay);
            var coupon = new Coupon
            {
                UserId=couponCreateRequest.UserId,
                CouponCode = couponCode,
                DiscountAmount = couponCreateRequest.DiscountAmount,
                ExpirationDate = expirationDate,
                IsActive = true,
            };

            var couponResult=await _couponRepository.CreateCouponAsync(coupon);

            var couponResponse = _mapper.Map<CouponResponse>(couponResult);

            return CustomResponse<CouponResponse>.Success(200, couponResponse);
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
