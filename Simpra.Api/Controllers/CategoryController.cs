using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Attribute;
using Simpra.Core.Entity;
using Simpra.Core.Jwt;
using Simpra.Core.Role;
using Simpra.Core.Service;
using Simpra.Schema.CategoryRR;
using Simpra.Service.Response;


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
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            var categoryResponse = _mapper.Map<CategoryResponse>(category);
            return CreateActionResult(CustomResponse<CategoryResponse>.Success(200, categoryResponse));
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetSingleCategoryByIdwithProducts([FromRoute] int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            var categoryResponse = _mapper.Map<CategoryWithProductResponse>(category);
            return CreateActionResult(CustomResponse<CategoryWithProductResponse>.Success(200, categoryResponse));
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Save([FromBody] CategoryCreateRequest categoryCreateRequest)
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserName)?.Value;
            var category = await _categoryService.AddAsync(_mapper.Map<Category>(categoryCreateRequest), username);
            var categoryResponse = _mapper.Map<CategoryResponse>(category);
            return CreateActionResult(CustomResponse<CategoryResponse>.Success(201, categoryResponse));
        }

        [HttpPut]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Update([FromBody] CategoryUpdateRequest categoryUpdateRequest)
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserName)?.Value;
            await _categoryService.UpdateAsync(_mapper.Map<Category>(categoryUpdateRequest), username);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            await _categoryService.RemoveAsync(category);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

    }
}
