using MassTransit;
using Serilog;
using Simpra.Core.Repository;
using Simpra.Core.Service;
using Simpra.Schema.BasketRR;
using Simpra.Service.Exceptions;
using Simpra.Service.Messages;
using Simpra.Service.Response;
using System.Text.Json;

namespace Simpra.Service.Service
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;
        private readonly IUserService _userService;
        private readonly IProductRepository _productRepository;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public BasketService(RedisService redisService, IUserService userService, IProductRepository productRepository, ISendEndpointProvider sendEndpointProvider)
        {
            _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException(nameof(sendEndpointProvider));
        }

        public async Task DeleteAsync(string userId)
        {
            try
            {
                var status = await _redisService.GetDb().KeyDeleteAsync(userId);
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

        public async Task<BasketResponse> GetBasketAsync(string userId)
        {
            try
            {
                var existBasket = await _redisService.GetDb().StringGetAsync(userId);
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

        public async Task SaveOrUpdateAsync(BasketRequest basketRequest,string userId)
        {
            try
            {
                // UserId Check => Token ile alırsak buna gerek yok artık
                var userCheck = _userService.GetById(userId);
                if (userCheck == null)
                    throw new NotFoundException($"User with id ({userId}) didn't find in the database.");

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
                var status = await _redisService.GetDb().StringSetAsync(userId, JsonSerializer.Serialize(basketRequest));

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

        public async Task CheckOutBasketAsync(BasketCheckOutRequest basketRequest,string userId)
        {
            //Kuyruk oluşturduk
            var sendEndPoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));
            var basket = await GetBasketAsync(userId);

            var createOrderMessageCommand = new CreateOrderMessageCommand()
            {
                UserId = userId,
                TotalPrice = basket.TotalPrice,
                CouponCode = basketRequest.CouponCode,
                CreditCard = basketRequest.CreditCard,
            };

            basket.BasketItems.ForEach(x =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItemDto
                {
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    ProductId = x.ProductId,
                });
            });

            //Mesajı gönderiyoruz. Order ayakta olmasa bile mesaj kuyrukta bekleyecek.
            await sendEndPoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);
        }
    }
}
