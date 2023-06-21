using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBasket()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var basketResponse = await _basketService.GetBasketAsync(userId);
            return CreateActionResult(CustomResponse<BasketResponse>.Success(200, basketResponse));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveOrUpdateBasket([FromBody] BasketRequest basketRequest)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            await _basketService.SaveOrUpdateAsync(basketRequest, userId);
            return CreateActionResult(CustomResponse<bool>.Success(204));
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteBasket()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            await _basketService.DeleteAsync(userId);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

        [Route("[action]")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CheckOut([FromBody] BasketCheckOutRequest basketRequest)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            await _basketService.CheckOutBasketAsync(basketRequest, userId);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

    }
}
