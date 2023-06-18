using Serilog;
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
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _unitOfWork = unitofWork ?? throw new ArgumentNullException(nameof(unitofWork));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public override async Task<Category> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _categoryRepository.GetByIdWithIncludeAsync(id,"Products");
                if (entity == null)
                    throw new NotFoundException($"Category ({id}) not found!");

                return entity;
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    Log.Warning(ex, "GetByIdAsync Exception - Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                }
                Log.Error(ex, "GetByIdAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
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
                    Log.Warning(ex, "RemoveAsync Exception - Client Side Error");
                    throw new ClientSideException($"Error message:{ex.Message}");
                }
                Log.Error(ex, "RemoveAsync Exception");
                throw new Exception($"Something went wrong! Error message:{ex.Message}");
            }
        }

    }
}
