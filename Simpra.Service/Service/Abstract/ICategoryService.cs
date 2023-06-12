using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Schema.ProductwithCategoryRR;
using Simpra.Service.Reponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Service.Service.Abstract
{
    public interface ICategoryService : IService<Category>
    {
        Task<CustomResponse<CategorywithProductResponse>> GetSingleCategoryByIdwithProductAsync(int categoryId);
        Task<bool> HasProducts(int categoryId);
    }
}
