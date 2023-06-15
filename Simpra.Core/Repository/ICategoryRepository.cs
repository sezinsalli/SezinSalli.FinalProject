using Simpra.Core.Entity;

namespace Simpra.Core.Repository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetSingleCategoryByIdwithProductAsync(int categoryId);
    }
}
