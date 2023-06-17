using Simpra.Schema.Basket;

namespace Simpra.Core.Service
{
    public interface IBasketService
    {
        Task<BasketResponse> GetBasketAsync(int userId);
        Task SaveOrUpdateAsync(BasketRequest basketDto);
        Task DeleteAsync(int userId);
    }
}
