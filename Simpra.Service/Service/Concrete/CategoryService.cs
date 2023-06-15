using AutoMapper;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.UnitofWork;
using Simpra.Schema.ProductwithCategoryRR;
using Simpra.Service.Reponse;
using Simpra.Service.Service.Abstract;

namespace Simpra.Service.Service.Concrete
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitofWork, ICategoryRepository categoryRepository, IMapper mapper) : base(repository, unitofWork)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;

        }

        public async Task<CustomResponse<CategorywithProductResponse>> GetSingleCategoryByIdwithProductAsync(int categoryId)
        {
            var category = await _categoryRepository.GetSingleCategoryByIdwithProductAsync(categoryId);
            var categoryDto = _mapper.Map<CategorywithProductResponse>(category);
            return CustomResponse<CategorywithProductResponse>.Success(200, categoryDto);
        }

        public async Task<bool> HasProducts(int categoryId)
        {
            var category = await _categoryRepository.GetSingleCategoryByIdwithProductAsync(categoryId);
            return category.Products.Any();
        }


    }
}
