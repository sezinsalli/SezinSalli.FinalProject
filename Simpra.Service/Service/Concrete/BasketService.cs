using Microsoft.Extensions.Logging;
using Simpra.Schema.Basket;
using Simpra.Service.Exceptions;
using Simpra.Service.Service.Abstract;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simpra.Core.Repository;
using Simpra.Core.Entity;
using Simpra.Core.Entity.Enum;

namespace Simpra.Service.Service.Concrete
{
    public class BasketService:IBasketService
    {
        private readonly RedisService _redisService;
        private readonly ILogger<BasketService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        public BasketService(RedisService redisService, ILogger<BasketService> logger,IUserRepository userRepository,IProductRepository productRepository)
        {
            _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task Delete(int userId)
        {
            var status = await _redisService.GetDb().KeyDeleteAsync(userId.ToString());

            if (!status)
            {
                _logger.LogError($"Basket with id ({userId}) didn't find in the database.");
                throw new NotFoundException($"Basket with id ({userId}) didn't find in the database.");
            }
        }

        public async Task<BasketResponse> GetBasket(int userId)
        {
            var existBasket = await _redisService.GetDb().StringGetAsync(userId.ToString());

            if (existBasket.IsNullOrEmpty)
            {
                _logger.LogError($"Basket with id ({userId}) didn't find in the database.");
                throw new NotFoundException($"Basket with id ({userId}) didn't find in the database.");
            }
            return JsonSerializer.Deserialize<BasketResponse>(existBasket);
        }

        public async Task SaveOrUpdate(BasketRequest basketRequest)
        {
            try
            {
                // UserId Check => Token ile alırsak buna gerek yok artık
                var userCheck = await _userRepository.AnyAsync(x => x.Id == basketRequest.UserId);

                if (!userCheck)
                {
                    throw new NotFoundException($"User with id ({basketRequest.UserId}) didn't find in the database.");
                }

                // Product Check => Product stoktan fazla olan bir ürün eklenmemesi için kontrol
                foreach (var basketItem in basketRequest.BasketItems)
                {
                    var product = await _productRepository.GetByIdAsync(basketItem.ProductId);

                    if (product == null)
                    {
                        throw new NotFoundException($"Product with id ({basketItem.ProductId}) didn't find in the database.");
                    }

                    if (product.Stock < basketItem.Quantity || product.Status == Status.OutOfStock)
                    {
                        throw new NotFoundException($"There are not enough products. Exist product ({product.Stock})");
                    }

                    if (!product.IsActive)
                    {
                        throw new NotFoundException($"Product with id ({basketItem.ProductId}) didn't active in the database.");
                    }
                }

                // Create or update basket 
                var status = await _redisService.GetDb().StringSetAsync(basketRequest.UserId.ToString(), JsonSerializer.Serialize(basketRequest));

                if (!status)
                {
                    _logger.LogError($"Basket didn't save or update in the database.");
                    throw new Exception($"Basket didn't save or update in the database.");
                }
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    throw new NotFoundException($"Basket didn't update in the redis. Error message:{ex.Message}");
                }

                if (ex is ClientSideException)
                {
                    throw new ClientSideException($"Basket didn't update in the redi. Error message:{ex.Message}");
                }

                throw new Exception($"Basket didn't update in the redi. Error message:{ex.Message}");
            }
        }
    }
}
