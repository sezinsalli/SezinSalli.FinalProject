using AutoMapper;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.UnitofWork;
using Simpra.Repository.Repositories;
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

    }
}
