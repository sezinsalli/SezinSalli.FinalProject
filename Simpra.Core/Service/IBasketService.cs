using Simpra.Schema.BasketRR;

namespace Simpra.Core.Service
{
    public interface IBasketService
    {
        Task<BasketResponse> GetBasketAsync(string userId);
        Task SaveOrUpdateAsync(BasketRequest basketRequest, string userId);
        Task DeleteAsync(string userId);
        Task CheckOutBasketAsync(BasketCheckOutRequest basketRequest, string userId);
    }
}
