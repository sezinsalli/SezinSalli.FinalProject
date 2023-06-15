using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simpra.Schema.Basket;
using Simpra.Service.Response;
using Simpra.Service.Service.Abstract;
using System.Net;

namespace Simpra.Api.Controllers
{
    public class BasketController : CustomBaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket(int userId)
        {
            var basketResponse = await _basketService.GetBasket(userId);
            return CreateActionResult(CustomResponse<BasketResponse>.Success(200, basketResponse));
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket([FromBody] BasketRequest basketRequest)
        {
            await _basketService.SaveOrUpdate(basketRequest);
            return CreateActionResult(CustomResponse<bool>.Success(204));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(int userId)
        {
            await _basketService.Delete(userId);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }
    }
}
