using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Schema.CategoryRR;
using Simpra.Schema.ProductRR;
using Simpra.Service.Reponse;
using Simpra.Service.Service.Abstract;

namespace Simpra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IService<Product> _service;
        private readonly IProductService _productService;


        public ProductController(IMapper mapper, IService<Product> service, IProductService productService)
        {
            _service = service;
            _mapper = mapper;
            _productService = productService;

        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();
            var productResponse = _mapper.Map<List<ProductResponse>>(products.ToList());

            return Ok(CustomResponse<List<ProductResponse>>.Success(200, productResponse));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var products = await _service.GetByIdAsync(id);

            if (products == null)
            {
                return CreateActionResult(CustomResponse<List<ProductResponse>>.Fail(400, "Bu id'ye sahip ürün bulunmamaktadır."));
            }

            var productsResponse = _mapper.Map<ProductResponse>(products);
            return CreateActionResult(CustomResponse<ProductResponse>.Success(200, productsResponse));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductRequest productRequest)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productRequest));
            var productRequests = _mapper.Map<List<ProductRequest>>(product);
            return CreateActionResult(CustomResponse<List<ProductRequest>>.Success(201, productRequests));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductResponse productResponse)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productResponse));

            return CreateActionResult(CustomResponse<List<NoContent>>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(product);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }


    }
}
