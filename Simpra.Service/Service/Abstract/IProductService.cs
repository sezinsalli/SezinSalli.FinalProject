using Simpra.Core.Entity;
using Simpra.Schema.ProductRR;

namespace Simpra.Service.Service.Abstract
{
    public interface IProductService : IService<Product>
    {
        Task<Product> ProductStockUpdateAsync(ProductStockUpdateRequest stockUpdateRequest);
        Task<List<Product>> GetActiveProducts();
    }
}
