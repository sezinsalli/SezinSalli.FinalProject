using Serilog;
using Simpra.Core.Repository;
using Simpra.Core.Service;
using Simpra.Schema.BasketRR;
using Simpra.Service.Exceptions;
using System.Text.Json;

namespace Simpra.Service.Service
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;
        private readonly IUserService _userService;
        private readonly IProductRepository _productRepository;

        public BasketService(RedisService redisService, IUserService userService, IProductRepository productRepository)
        {
            _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task DeleteAsync(int userId)
        {
            try
            {
                var status = await _redisService.GetDb().KeyDeleteAsync(userId.ToString());
                if (!status)
                    throw new NotFoundException($"Basket with id ({userId}) didn't find in the database.");
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    Log.Warning(ex, "DeleteAsync Exception - Not Found Error");
                    throw new NotFoundException($"Basket didn't delete in the redis. Error message:{ex.Message}");
                }
                Log.Error(ex, "DeleteAsync Exception");
                throw new Exception($"Something went wrong! Error message:{ex.Message}");
            }
        }

        public async Task<BasketResponse> GetBasketAsync(int userId)
        {
            try
            {
                var existBasket = await _redisService.GetDb().StringGetAsync(userId.ToString());
                if (existBasket.IsNullOrEmpty)
                    throw new NotFoundException($"Basket with id ({userId}) didn't find in the database.");

                var basketResponse = JsonSerializer.Deserialize<BasketResponse>(existBasket);
                return basketResponse;
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    Log.Warning(ex, "GetBasketAsync Exception - Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                }
                Log.Error(ex, "GetBasketAsync Exception");
                throw new Exception($"Something went wrong!. Error message:{ex.Message}");
            }
        }

        public async Task SaveOrUpdateAsync(BasketRequest basketRequest)
        {
            try
            {
                // UserId Check => Token ile alırsak buna gerek yok artık
                var userCheck = await _userService.GetByIdAsync(basketRequest.UserId);
                if (userCheck == null)
                    throw new NotFoundException($"User with id ({basketRequest.UserId}) didn't find in the database.");

                // Product Check => Product stoktan fazla olan bir ürün eklenmemesi için kontrol
                foreach (var basketItem in basketRequest.BasketItems)
                {
                    var product = await _productRepository.GetByIdAsync(basketItem.ProductId);
                    if (product == null)
                        throw new NotFoundException($"Product with id ({basketItem.ProductId}) didn't find in the database.");

                    if (product.Stock < basketItem.Quantity)
                        throw new NotFoundException($"There are not enough products. Exist product ({product.Stock})");

                    if (!product.IsActive)
                        throw new NotFoundException($"Product with id ({basketItem.ProductId}) didn't active in the database.");
                }
                // Create or update basket 
                var status = await _redisService.GetDb().StringSetAsync(basketRequest.UserId.ToString(), JsonSerializer.Serialize(basketRequest));

                if (!status)
                    throw new Exception($"Basket didn't save or update in the database.");
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    Log.Warning(ex, "SaveOrUpdateAsync Exception - Not Found Error");
                    throw new NotFoundException($"Basket didn't update in the redis. Error message:{ex.Message}");
                }
                if (ex is ClientSideException)
                {
                    Log.Warning(ex, "SaveOrUpdateAsync Exception - Client Side Error");
                    throw new ClientSideException($"Basket didn't update in the redi. Error message:{ex.Message}");
                }
                Log.Error(ex, "SaveOrUpdateAsync Exception");
                throw new Exception($"Basket didn't update in the redi. Error message:{ex.Message}");
            }
        }
    }
}
