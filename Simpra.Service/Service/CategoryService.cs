using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.Service;
using Simpra.Core.UnitofWork;
using Simpra.Service.Exceptions;

namespace Simpra.Service.Service
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitofWork, ICategoryRepository categoryRepository, IProductRepository productRepository) : base(categoryRepository, unitofWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitofWork;
            _productRepository = productRepository;
        }

        public async Task<Category> GetCategoryByIdWithProductAsync(int categoryId)
        {
            try
            {
                var categoryCheck = await _categoryRepository.AnyAsync(x => x.Id == categoryId);
                if (!categoryCheck)
                    throw new NotFoundException($"Category with ({categoryId}) not found!");

                var category = _categoryRepository.GetByIdWithInclude(categoryId,"Products");
                return category;
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    throw new NotFoundException($"Error Message: {ex.Message}");
                }
                throw new Exception($"Something went wrong! Error message:{ex.Message}");
            }
        }

        public override async Task RemoveAsync(Category entity)
        {
            try
            {
                var productCheck = await _productRepository.AnyAsync(x => x.CategoryId == entity.Id);
                if (productCheck)
                    throw new ClientSideException($"Category with ({entity.Id}) cannot delete! Firstly remove products with related this category!");

                _categoryRepository.Remove(entity);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                if (ex is ClientSideException)
                {
                    throw new ClientSideException($"Error message:{ex.Message}");
                }
                throw new Exception($"Something went wrong! Error message:{ex.Message}");
            }
        }

    }
}
