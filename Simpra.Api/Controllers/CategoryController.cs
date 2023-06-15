using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Entity;
using Simpra.Schema.CategoryRR;
using Simpra.Schema.ProductwithCategoryRR;
using Simpra.Service.Reponse;
using Simpra.Service.Service.Abstract;


namespace Simpra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public CategoryController(IMapper mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var categories = await _categoryService.GetAllAsync();
            var categoryResponse = _mapper.Map<List<CategoryResponse>>(categories.ToList());
            return CreateActionResult(CustomResponse<List<CategoryResponse>>.Success(200, categoryResponse));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            var categoryResponse = _mapper.Map<CategoryResponse>(category);
            return CreateActionResult(CustomResponse<CategoryResponse>.Success(200, categoryResponse));
        }

        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSingleCategoryByIdwithProducts(int categoryId)
        {
            var category = await _categoryService.GetSingleCategoryByIdWithProductsAsync(categoryId);
            var categoryResponse = _mapper.Map<CategoryWithProductResponse>(category);
            return CreateActionResult(CustomResponse<CategoryWithProductResponse>.Success(200, categoryResponse));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] CategoryCreateRequest categoryCreateRequest)
        {
            var category = await _categoryService.AddAsync(_mapper.Map<Category>(categoryCreateRequest));
            var categoryResponse = _mapper.Map<CategoryResponse>(category);
            return CreateActionResult(CustomResponse<CategoryResponse>.Success(201, categoryResponse));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CategoryUpdateRequest categoryUpdateRequest)
        {
            await _categoryService.UpdateAsync(_mapper.Map<Category>(categoryUpdateRequest));
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _categoryService.RemoveCategoryWithCheckProduct(id);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

    }
}
