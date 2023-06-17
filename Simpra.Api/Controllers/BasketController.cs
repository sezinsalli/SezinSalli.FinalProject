using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Simpra.Api.Messages;
using Simpra.Core.Service;
using Simpra.Schema.BasketRR;
using Simpra.Service.Response;

namespace Simpra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : CustomBaseController
    {
        private readonly IBasketService _basketService;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public BasketController(IBasketService basketService, ISendEndpointProvider sendEndpointProvider)
        {
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException(nameof(sendEndpointProvider));
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket(int userId)
        {
            var basketResponse = await _basketService.GetBasketAsync(userId);
            return CreateActionResult(CustomResponse<BasketResponse>.Success(200, basketResponse));
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket([FromBody] BasketRequest basketRequest)
        {
            await _basketService.SaveOrUpdateAsync(basketRequest);
            return CreateActionResult(CustomResponse<bool>.Success(204));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(int userId)
        {
            await _basketService.DeleteAsync(userId);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

        // Basket Check Out UserId ile de alabilir

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> CheckOut([FromBody] BasketCheckOutRequest basketRequest)
        {
            //Kuyruk oluşturduk
            var sendEndPoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand()
            {
                UserId = basketRequest.UserId,
                TotalPrice = basketRequest.TotalPrice,
                CouponCode = basketRequest.CouponCode,
            };

            basketRequest.BasketItems.ForEach(x =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItemDto
                {
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    ProductId = x.ProductId,
                });
            });

            //Mesajı gönderiyoruz. Order ayakta olmasa bile mesaj kuyrukta bekleyecek.
            await sendEndPoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

    }
}
