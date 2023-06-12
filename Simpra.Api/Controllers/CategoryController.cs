using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Schema.CategoryRR;
using Simpra.Service.Exceptions;
using Simpra.Service.Reponse;
using Simpra.Service.Service.Abstract;


namespace Simpra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IService<Category> _service;
        private readonly ICategoryService _categoryService;
       

        public CategoryController(IMapper mapper, IService<Category> service, ICategoryService categoryService)
        {
            _service = service;
            _mapper = mapper;
            _categoryService = categoryService;
            
        }

        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSingleCategoryByIdwithProducts(int categoryId)
        {
            return CreateActionResult(await _categoryService.GetSingleCategoryByIdwithProductAsync(categoryId));
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var categories = await _service.GetAllAsync();
            var categoryResponse = _mapper.Map<List<CategoryResponse>>(categories.ToList());

            return Ok(CustomResponse<List<CategoryResponse>>.Success(200, categoryResponse));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var categories = await _service.GetByIdAsync(id);

            if (categories == null)
            {
                return CreateActionResult(CustomResponse<CategoryResponse>.Fail(400, "Bu id'ye sahip ürün bulunmamaktadır."));
            }

            var categoriesResponse = _mapper.Map<CategoryResponse>(categories);
            return CreateActionResult(CustomResponse<CategoryResponse>.Success(200, categoriesResponse));
        }
        
        [HttpPost]
        public async Task<IActionResult> Save(CategoryCreateRequest categoryCreateRequest)
        {
            var category = await _service.AddAsync(_mapper.Map<Category>(categoryCreateRequest));
            var categoryResponse = _mapper.Map<CategoryResponse>(category);
            return CreateActionResult(CustomResponse<CategoryResponse>.Success(201, categoryResponse));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryUpdateRequest categoryUpdateRequest)
        {
            await _service.UpdateAsync(_mapper.Map<Category>(categoryUpdateRequest));

            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                var category = await _service.GetByIdAsync(id);
                if (category == null)
                {
                    throw new NotFoundException("Category not found"); 
                }

                // Check if the category has any associated products
                var hasProducts = await _categoryService.HasProducts(id);
                if (hasProducts)
                {
                    var errorResponse = CustomResponse<string>.Fail(400, "The category cannot be deleted because it has associated products.");
                    return BadRequest(errorResponse); 
                }

                
                await _service.RemoveAsync(category);

                var successResponse = CustomResponse<string>.Success(204);
                return NoContent(); 
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message); // Return a 404 Not Found response with the exception message
            }
        }




    }
}
