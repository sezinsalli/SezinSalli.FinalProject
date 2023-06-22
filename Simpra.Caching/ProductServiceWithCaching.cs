using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.Service;
using Simpra.Core.UnitofWork;
using Simpra.Schema.ProductRR;
using Simpra.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Caching
{
    //"Decorator design pattern" veya ona çok yakın olan "Proxy design pattern" implementasyonunu gerçekleştireceğiz.
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "productsCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductServiceWithCaching(IMapper mapper, IMemoryCache memoryCache, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;

            //Uygulama ayağa kalktığında cache de yok ise buraya girerek oluşturacak daha sonra tekrar girmeyecek.
            if (!_memoryCache.TryGetValue(CacheProductKey, out _))
            {
                _memoryCache.Set(CacheProductKey, _productRepository.GetAll().ToList());
            }
        }

        //Çok sık erişip çok fazla güncellemediğimiz data "Cache" yapılması daha sağlıklıdır.
        public async Task<Product> AddAsync(Product product, string changedBy)
        {
            try
            {
                product.CreatedBy = changedBy;
                await _productRepository.AddAsync(product);
                await _unitOfWork.CompleteAsync();
                //Database'e ekleme yaptıktan sonra tekrar "Cache" leme işlemini yapıyoruz.
                CacheAllProductsAsync();
                return product;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AddAsync Exception");
                throw new Exception($"Product cannot create. Error message:{ex.Message}");
            }
        }
        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> products, string changedBy)
        {
            try
            {
                foreach (var product in products)
                {
                    product.CreatedBy = changedBy;
                }
                await _productRepository.AddRangeAsync(products);
                await _unitOfWork.CompleteAsync();
                //Database'e ekleme yaptıktan sonra tekrar "Cache" leme işlemini yapıyoruz.
                CacheAllProductsAsync();
                return products;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AddRangeAsync Exception");
                throw new Exception($"Products cannot create. Error message:{ex.Message}");
            }
        }
        public async Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            try
            {
                return await Task.FromResult(_memoryCache.Get<List<Product>>(CacheProductKey).Any(expression.Compile()));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AnyAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }
        public Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = _memoryCache.Get<List<Product>>(CacheProductKey);
                return Task.FromResult<IEnumerable<Product>>(products);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetAllAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }
        public Task<Product> GetByIdAsync(int id)
        {
            try
            {
                var product = _memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x => x.Id == id);
                if (product == null)
                    throw new NotFoundException($"Product ({id}) not found!");

                return Task.FromResult<Product>(product);
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
        public async Task RemoveAsync(Product product)
        {
            try
            {
                _productRepository.Remove(product);
                await _unitOfWork.CompleteAsync();
                CacheAllProductsAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "RemoveAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }
        public async Task RemoveRangeAsync(IEnumerable<Product> products)
        {
            try
            {
                _productRepository.RemoveRange(products);
                await _unitOfWork.CompleteAsync();
                CacheAllProductsAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "RemoveRangeAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }
        public async Task<Product> ProductStockUpdateAsync(ProductStockUpdateRequest stockUpdateRequest)
        {
            try
            {
                var product = _memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x => x.Id == stockUpdateRequest.Id);

                if (product == null)
                    throw new NotFoundException($"Product with ({stockUpdateRequest.Id}) not found!");

                // Stok artışını gerçekleştir
                product.Stock += stockUpdateRequest.StockChange;

                // Stok 0 dan küçük olmamalı
                if (product.Stock < 0)
                    throw new NotFoundException("Product stock cannot be less than 0!");

                // Stok 0 ise ürün pasif duruma geçiyor ve status durumu otomatik değiştiriliyor.
                if (product.Stock == 0)
                {
                    product.Status = Core.Enum.ProductStatus.OutOfStock;
                    product.IsActive = false;
                }

                _productRepository.Update(product);
                await _unitOfWork.CompleteAsync();
                CacheAllProductsAsync();
                return product;
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    Log.Warning(ex, "ProductStockUpdateAsync Exception - Not Found Error");
                    throw new NotFoundException($"Product didn't update in the database. Error message:{ex.Message}");
                }

                if (ex is ClientSideException)
                {
                    Log.Warning(ex, "ProductStockUpdateAsync Exception - Client Side Error");
                    throw new ClientSideException($"Product didn't update in the database. Error message:{ex.Message}");
                }
                Log.Error(ex, "ProductStockUpdateAsync Exception");
                throw new Exception($"Product didn't update in the database. Error message:{ex.Message}");
            }
        }
        public async Task UpdateAsync(Product product, string changedBy)
        {
            try
            {
                var response = await Task.FromResult(_memoryCache.Get<List<Product>>(CacheProductKey).Any(x => x.Id == product.Id));
                if (!response)
                    throw new NotFoundException($"Product ({product.Id}) not found!");

                product.UpdatedBy = changedBy;
                _productRepository.Update(product);
                await _unitOfWork.CompleteAsync();
                CacheAllProductsAsync();
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    Log.Warning(ex, "UpdateAsync Exception - Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                }
                Log.Error(ex, "UpdateAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }
        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            try
            {
                //Expression 'ı function'a çervirmek için "Compile" metotunu kullanıyoruz.
                return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Where Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }
        public IEnumerable<Product> WhereWithInclude(Expression<Func<Product, bool>> expression, params string[] includes)
        {
            try
            {
                return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "WhereWithInclude Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }

        //Çağırdığımızda Cache lemek için burada bir metot tanımladık.
        public void CacheAllProductsAsync()
        {
            _memoryCache.Set(CacheProductKey, _productRepository.GetAll().ToList());
        }
    }
}
