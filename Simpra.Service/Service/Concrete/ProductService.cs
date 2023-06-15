using Microsoft.EntityFrameworkCore;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.UnitofWork;
using Simpra.Schema.ProductRR;
using Simpra.Service.Exceptions;
using Simpra.Service.Service.Abstract;

namespace Simpra.Service.Service.Concrete
{
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : base(productRepository, unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> ProductStockUpdateAsync(ProductStockUpdateRequest stockUpdateRequest)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(stockUpdateRequest.Id);

                if (product == null)
                {
                    throw new NotFoundException($"Product with ({stockUpdateRequest.Id}) not found!");
                }

                // Stok artışını gerçekleştir
                product.Stock = product.Stock + stockUpdateRequest.StockChange;

                // Stok 0 dan küçük olmamalı
                if (product.Stock < 0)
                {
                    throw new NotFoundException("Product stock cannot be less than 0!");
                }

                // Stok 0 ise ürün pasif duruma geçiyor ve status durumu otomatik değiştiriliyor.
                if (product.Stock == 0)
                {
                    product.Status = Core.Entity.Enum.Status.OutOfStock;
                    product.IsActive = false;
                }

                _productRepository.Update(product);
                await _unitOfWork.CompleteAsync();
                return product;
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    throw new NotFoundException($"Product didn't update in the database. Error message:{ex.Message}");
                }

                if (ex is ClientSideException)
                {
                    throw new ClientSideException($"Product didn't update in the database. Error message:{ex.Message}");
                }

                throw new Exception($"Product didn't update in the database. Error message:{ex.Message}");
            }
        }

        public async Task<List<Product>> GetActiveProducts()
        {
            try
            {
                var products = await _productRepository.Where(x => x.IsActive).ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception($"Product didn't get from the database. Error message:{ex.Message}");
            }
        }




    }
}
