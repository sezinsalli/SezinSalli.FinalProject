using Simpra.Core.Entity;

namespace Simpra.Service.Service.Abstract
{
    public interface ICategoryService : IService<Category>
    {
        Task<Category> GetSingleCategoryByIdWithProductsAsync(int categoryId);
        Task RemoveCategoryWithCheckProduct(int id);
    }
}
