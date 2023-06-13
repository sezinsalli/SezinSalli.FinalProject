using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Schema.ProductRR;
using Simpra.Schema.ProductwithCategoryRR;
using Simpra.Service.Reponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Service.Service.Abstract
{
    public interface IProductService:IService<Product>
    {
        Task<Product> ProductStockUpdateAsync(ProductStockUpdateRequest stockUpdateRequest);
    }
}
