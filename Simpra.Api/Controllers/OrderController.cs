using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Schema.OrderRR;
using Simpra.Service.Response;

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
            var orders = await _service.GetAllAsync();
            var orderResponse = _mapper.Map<List<OrderResponse>>(orders);
            return CreateActionResult(CustomResponse<List<OrderResponse>>.Success(200, orderResponse));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] int status)
        {
            var orderResult = await _service.UpdateOrderStatusAsync(id, status);
            var orderResponse = _mapper.Map<OrderResponse>(orderResult);
            return CreateActionResult(CustomResponse<OrderResponse>.Success(201, orderResponse));
        }

    }
}
