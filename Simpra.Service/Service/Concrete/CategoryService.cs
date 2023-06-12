using AutoMapper;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.UnitofWork;
using Simpra.Repository.Repositories;
using Simpra.Schema.ProductwithCategoryRR;
using Simpra.Service.Reponse;
using Simpra.Service.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<CustomResponse<List<ProductwithCategoryResponse>>> GetProductsByCategoryId(int categoryId)
        {
            var category = await _categoryRepository.GetProductsByCategoryId(categoryId);

            var categoryDto = _mapper.Map<List<ProductwithCategoryResponse>>(category);

            return CustomResponse<List<ProductwithCategoryResponse>>.Success(200, categoryDto);
        }
        public async Task<bool> HasProducts(int categoryId)
        {
            var products = await _categoryRepository.GetProductsByCategoryId(categoryId);
            return products.Any();
        }
    }
}
