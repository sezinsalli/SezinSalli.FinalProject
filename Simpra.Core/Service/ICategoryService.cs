using Simpra.Core.Entity;

namespace Simpra.Core.Service
{
    public interface ICategoryService : IBaseService<Category>
    {
        Task<Category> GetSingleCategoryByIdWithProductsAsync(int categoryId);
        Task RemoveCategoryWithCheckProductAsync(int id);
    }
}
