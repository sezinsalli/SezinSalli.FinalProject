using Simpra.Core.Entity;
using Simpra.Schema.ProductRR;

namespace Simpra.Core.Service
{
    public interface IProductService : IBaseService<Product>
    {
        Task<Product> ProductStockUpdateAsync(ProductStockUpdateRequest stockUpdateRequest);
    }
}
