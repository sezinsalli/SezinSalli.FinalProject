using Simpra.Schema.Basket;

namespace Simpra.Service.Service.Abstract
{
    public interface IBasketService
    {
        Task<BasketResponse> GetBasket(int userId);
        Task SaveOrUpdate(BasketRequest basketDto);
        Task Delete(int userId);
    }
}
