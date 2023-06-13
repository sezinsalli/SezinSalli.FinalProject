using AutoMapper;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.UnitofWork;
using Simpra.Service.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Service.Service.Concrete
{
    public class OrderService : Service<Order>, IOrderService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        public OrderService(IGenericRepository<Order> repository, IUnitOfWork unitofWork, ICouponRepository couponRepository, IMapper mapper) : base(repository, unitofWork)
        {
            _mapper = mapper;
            _couponRepository = couponRepository;
        }

    }

}
