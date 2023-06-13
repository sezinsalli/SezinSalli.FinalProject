using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.UnitofWork;
using Simpra.Repository.Repositories;
using Simpra.Schema.OrderRR;
using Simpra.Schema.ProductwithCategoryRR;
using Simpra.Service.Reponse;
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
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderService(IGenericRepository<Order> repository, IUnitOfWork unitofWork, IOrderRepository orderRepository, IMapper mapper) : base(repository, unitofWork)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public List<Order> GetOrdersWithOrderDetails()
        {
            return _orderRepository.GetOrdersWithOrderDetails();

        }

    }

}
