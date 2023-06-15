using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Schema.ProductwithCategoryRR;
using Simpra.Service.Reponse;

namespace Simpra.Service.Service.Abstract
{
    public interface ICategoryService : IService<Category>
    {
        Task<CustomResponse<CategorywithProductResponse>> GetSingleCategoryByIdwithProductAsync(int categoryId);
        Task<bool> HasProducts(int categoryId);
    }
}
