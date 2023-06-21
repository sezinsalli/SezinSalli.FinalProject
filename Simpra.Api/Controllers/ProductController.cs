using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Entity;
using Simpra.Core.Jwt;
using Simpra.Core.Role;
using Simpra.Core.Service;
using Simpra.Schema.ProductRR;
using Simpra.Service.Response;

namespace Simpra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _productService.GetAllAsync();
            var productResponse = _mapper.Map<List<ProductResponse>>(products.ToList());
            return Ok(CustomResponse<List<ProductResponse>>.Success(200, productResponse));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            var productResponse = _mapper.Map<ProductResponse>(product);
            return CreateActionResult(CustomResponse<ProductResponse>.Success(200, productResponse));
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Save([FromBody] ProductCreateRequest productCreateRequest)
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserName)?.Value;
            var product = await _productService.AddAsync(_mapper.Map<Product>(productCreateRequest), username);
            var productResponse = _mapper.Map<ProductResponse>(product);
            return CreateActionResult(CustomResponse<ProductResponse>.Success(201, productResponse));
        }

        [HttpPut]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Update([FromBody] ProductUpdateRequest productUpdateRequest)
        {
            var username = User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserName)?.Value;
            await _productService.UpdateAsync(_mapper.Map<Product>(productUpdateRequest), username);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            await _productService.RemoveAsync(product);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

        [HttpPut("[action]")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> IncreaseOrDecreaseStock([FromBody] ProductStockUpdateRequest stockUpdateRequest)
        {
            var product = await _productService.ProductStockUpdateAsync(stockUpdateRequest);
            var productResponse = _mapper.Map<ProductResponse>(product);
            return CreateActionResult(CustomResponse<ProductResponse>.Success(200, productResponse));
        }
    }
}
