using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Schema.ProductRR;

namespace Simpra.Service.Service.Abstract
{
    public interface IProductService : IService<Product>
    {
        Task<Product> ProductStockUpdateAsync(ProductStockUpdateRequest stockUpdateRequest);
    }
}
