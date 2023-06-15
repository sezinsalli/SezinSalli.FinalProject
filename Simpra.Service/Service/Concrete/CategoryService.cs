using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.UnitofWork;
using Simpra.Service.Exceptions;
using Simpra.Service.Service.Abstract;

namespace Simpra.Service.Service.Concrete
{
    public class CategoryService : Service<Category>, ICategoryService
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

        public async Task<Category> GetSingleCategoryByIdWithProductsAsync(int categoryId)
        {
            try
            {
                var categoryCheck = await _categoryRepository.AnyAsync(x => x.Id == categoryId);

                if (!categoryCheck)
                {
                    throw new NotFoundException($"Category with ({categoryId}) not found!");
                }

                var category = await _categoryRepository.GetSingleCategoryByIdwithProductAsync(categoryId);
                return category;
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    throw new NotFoundException($"Category with ({categoryId}) not found!");
                }

                throw new Exception($"Something went wrong! Error message:{ex.Message}");
            }
        }

        public async Task RemoveCategoryWithCheckProduct(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);

                if (category == null)
                {
                    throw new NotFoundException($"Category with ({id}) not found!");
                }

                var productCheck = await _productRepository.AnyAsync(x => x.CategoryId == id);

                if (productCheck)
                {
                    throw new ClientSideException($"Category with ({id}) cannot delete! Firstly remove products with related this category!");
                }

                _categoryRepository.Remove(category);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    throw new NotFoundException($"Category cannot delete!Error message:{ex.Message}");
                }

                if (ex is ClientSideException)
                {
                    throw new ClientSideException($"Category cannot delete!Error message:{ex.Message}");
                }

                throw new Exception($"Something went wrong! Error message:{ex.Message}");
            }
        }
    }
}
