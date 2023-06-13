using AutoMapper;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.UnitofWork;
using Simpra.Repository.Repositories;
using Simpra.Schema.ProductRR;
using Simpra.Schema.ProductwithCategoryRR;
using Simpra.Service.Exceptions;
using Simpra.Service.Reponse;
using Simpra.Service.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Service.Service.Concrete
{
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IProductRepository productRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _mapper = mapper;
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
                    throw new NotFoundException($"{stockUpdateRequest.Id} not found");
                }

                if (product.Status==Core.Entity.Enum.Status.OutOfStock)
                {
                    throw new NotFoundException($"{stockUpdateRequest.Id} - Ürün stokta bulunmamaktadır.");
                }

                // Stok artışını gerçekleştir
                product.Stock = product.Stock + stockUpdateRequest.StockChange;

                // Stok 0 dan küçük olmamalı
                if (product.Stock < 0)
                {
                    throw new ClientSideException("Stokta bulunan miktardan fazla ürün çıkışı yapmaya çalışmayınız!");
                }

                // Stok 0 ise ürün pasif duruma geçiyor ve status durumu otomatik değiştiriliyor.
                if (product.Stock == 0)
                {
                    product.Status = Core.Entity.Enum.Status.OutOfStock;
                    product.IsActive = false;
                }

                // Veritabanında güncelleme işlemi yapabilirsiniz
                _productRepository.Update(product);
                await _unitOfWork.CompleteAsync();
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception($"Product didn't update in the database. Error message:{ex.Message}");
            }
        }






    }
}
