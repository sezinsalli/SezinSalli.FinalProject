using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Entity;
using Simpra.Schema.OrderRR;
using Simpra.Service.Response;
using Simpra.Service.Service.Abstract;

namespace Simpra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _service;

        public OrderController(IMapper mapper, IOrderService service)
        {
            _service = service;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var orders = _service.GetOrdersWithOrderDetails();

            var orderResponse = _mapper.Map<List<OrderResponse>>(orders);

            return Ok(CustomResponse<List<OrderResponse>>.Success(200, orderResponse));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //Include
            var order = await _service.GetByIdAsync(id);

            if (order == null)
            {
                return CreateActionResult(CustomResponse<OrderResponse>.Fail(400, "Bu id'ye sahip ürün bulunmamaktadır."));
            }

            var orderResponse = _mapper.Map<OrderResponse>(order);

            return CreateActionResult(CustomResponse<OrderResponse>.Success(200, orderResponse));
        }

        [HttpPost]
        public async Task<IActionResult> Save(OrderCreateRequest orderCreateRequest)
        {
            var order = _mapper.Map<Order>(orderCreateRequest);

            var orderResult = await _service.CreateOrderAsync(order);

            var orderResponse = _mapper.Map<OrderResponse>(orderResult);

            return CreateActionResult(CustomResponse<OrderResponse>.Success(201, orderResponse));
        }
    }
}
