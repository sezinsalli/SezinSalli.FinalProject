using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simpra.Api.Helper;
using Simpra.Core.Entity;
using Simpra.Core.Enum;
using Simpra.Core.Jwt;
using Simpra.Core.Role;
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
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> All()
        {
            var orders = await _service.GetAllAsync();
            var orderResponse = _mapper.Map<List<OrderResponse>>(orders);
            return CreateActionResult(CustomResponse<List<OrderResponse>>.Success(200, orderResponse));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var order = await _service.GetByIdAsync(id);
            var orderResponse = _mapper.Map<OrderResponse>(order);
            return CreateActionResult(CustomResponse<OrderResponse>.Success(200, orderResponse));
        }

        [HttpGet("[action]")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult GetOrdersByStatus([FromQuery] int status)
        {
            var orders = _service.WhereWithInclude(x => x.Status == (OrderStatus)status, "OrderDetails");
            var orderResponse = _mapper.Map<List<OrderResponse>>(orders);
            return CreateActionResult(CustomResponse<List<OrderResponse>>.Success(200, orderResponse));
        }

        [HttpGet("[action]")]
        [Authorize]
        public IActionResult GetByUserId()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserId)?.Value;
            var orders = _service.WhereWithInclude(x => x.UserId == userId, "OrderDetails");
            var orderResponse = _mapper.Map<List<OrderResponse>>(orders);
            return CreateActionResult(CustomResponse<List<OrderResponse>>.Success(200, orderResponse));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Save([FromBody] OrderCreateRequest orderCreateRequest)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserId)?.Value;

            // Kredi kartı bilgilerini hashleme => Gerçek proejelerde Stripe.net gibi araçlar kullanılabilir.
            string hashedCreditCard = CreditCardHashHelper.HashCreditCardInfo(orderCreateRequest.CreditCard);

            var orderResult = await _service
                .CreateOrderAsync(_mapper.Map<Order>(orderCreateRequest), userId, hashedCreditCard);

            var orderResponse = _mapper.Map<OrderResponse>(orderResult);
            return CreateActionResult(CustomResponse<OrderResponse>.Success(201, orderResponse));
        }

        [HttpPut("[action]/{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> UpdateStatus([FromRoute] int id, [FromQuery] int status)
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserName)?.Value;
            var orderResult = await _service.UpdateOrderStatusAsync(id, status, username);
            var orderResponse = _mapper.Map<OrderResponse>(orderResult);
            return CreateActionResult(CustomResponse<OrderResponse>.Success(200, orderResponse));
        }

    }
}
