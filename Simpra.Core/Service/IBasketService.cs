using Simpra.Schema.BasketRR;

namespace Simpra.Core.Service
{
    public interface IBasketService
    {
        Task<BasketResponse> GetBasketAsync(int userId);
        Task SaveOrUpdateAsync(BasketRequest basketDto);
        Task DeleteAsync(int userId);
    }
}
