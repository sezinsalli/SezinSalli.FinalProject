using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.Service;
using Simpra.Core.UnitofWork;
using Simpra.Schema.OrderRR;
using Simpra.Service.Exceptions;

namespace Simpra.Service.Service
{
    public class OrderService : BaseService<Order>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IGenericRepository<Order> repository, IUnitOfWork unitofWork, IOrderRepository orderRepository, IMapper mapper, IGenericRepository<User> userRepository, ICouponRepository couponRepository, IProductRepository productRepository) : base(repository, unitofWork)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _unitOfWork = unitofWork;
            _userRepository = userRepository;
            _couponRepository = couponRepository;
            _productRepository = productRepository;
        }

        public override async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var entities = await _orderRepository.GetAllWithIncludeAsync("OrderDetails");
                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }

        public override async Task<Order> GetByIdAsync(int id)
        {
            try
            {
                var order = await _orderRepository.GetByIdWithIncludeAsync(id,"OrderDetails");
                if (order == null)
                    throw new NotFoundException($"Order ({id}) not found!");

                return order;
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                }
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Check user
            var user = await _userRepository.GetByIdAsync(order.UserId);

            if (user == null)
                throw new NotFoundException($"Order with userId ({order.UserId}) didn't find in the database.");

            // Calculate total price
            order.TotalAmount = order.OrderDetails.Sum(x => x.Quantity * x.UnitPrice);
            order.BillingAmount = order.TotalAmount;

            // Coupon Using
            if (order.CouponCode != "" && order.CouponCode != null)
            {
                var coupon = await _couponRepository.Where(x => x.CouponCode == order.CouponCode).SingleOrDefaultAsync();

                if (coupon == null)
                    throw new NotFoundException($"Coupon with couponCode ({order.CouponCode}) didn't find in the database.");

                CheckCouponUsing(ref coupon, ref order);
            }
            else
            {
                order.CouponCode = "No Coupon";
            }

            // Digital Wallet Kullanımı
            CheckDigitalWalletUsing(ref user, ref order);

            // Kredi Kartı Kullanımı
            if (order.BillingAmount > 0)
            {
                CheckCreditCardUsing();
            }

            // Puan Kazanma
            var earnedPoints = await CheckEarnPoints(order, user);
            user.DigitalWalletBalance += earnedPoints;

            order.IsActive = true;
            _userRepository.Update(user);
            await _orderRepository.AddAsync(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }

        public async Task<Order> CreateOrderFromMessage(OrderCreateRequest orderRequest)
        {
            var order = _mapper.Map<Order>(orderRequest);

            // Check user
            var user = await _userRepository.GetByIdAsync(order.UserId);

            if (user == null)
            {
                throw new NotFoundException($"Order with userId ({order.UserId}) didn't find in the database.");
            }

            // Calculate total price
            order.TotalAmount = order.OrderDetails.Sum(x => x.Quantity * x.UnitPrice);
            order.BillingAmount = order.TotalAmount;

            // Coupon Using
            if (order.CouponCode != "" && order.CouponCode != null)
            {
                // Get Coupon
                var coupon = await _couponRepository.Where(x => x.CouponCode == order.CouponCode).SingleOrDefaultAsync();

                if (coupon == null)
                {
                    throw new NotFoundException($"Coupon with couponCode ({order.CouponCode}) didn't find in the database.");
                }

                // Check Coupon
                CheckCouponUsing(ref coupon, ref order);
            }
            else
            {
                order.CouponCode = "No Coupon";
            }

            // Digital Wallet Kullanımı
            CheckDigitalWalletUsing(ref user, ref order);

            // Kredi Kartı Kullanımı
            if (order.BillingAmount > 0)
            {

            }

            // Puan Kazanma
            var earnedPoints = await CheckEarnPoints(order, user);
            user.DigitalWalletBalance += earnedPoints;

            order.IsActive = true;
            _userRepository.Update(user);
            await _orderRepository.AddAsync(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }

        private void CheckDigitalWalletUsing(ref User user, ref Order order)
        {
            if (order.BillingAmount > 0 && order.BillingAmount >= user.DigitalWalletBalance)
            {
                order.BillingAmount -= user.DigitalWalletBalance;
                order.WalletAmount = user.DigitalWalletBalance;
                user.DigitalWalletBalance = 0;
            }

            if (order.BillingAmount > 0 && order.BillingAmount < user.DigitalWalletBalance)
            {
                user.DigitalWalletBalance -= order.BillingAmount;
                order.WalletAmount = order.BillingAmount;
                order.BillingAmount = 0;
            }
        }

        private void CheckCouponUsing(ref Coupon coupon, ref Order order)
        {
            if (coupon.UserId != order.UserId)
                throw new ClientSideException($"Coupon and User Id doesn't match!");

            if (coupon.IsActive != true)
                throw new ClientSideException($"Coupon isn't active!");

            if (coupon.ExpirationDate < DateTime.Now)
            {
                coupon.IsActive = false;
                _couponRepository.Update(coupon);
                _unitOfWork.CompleteAsync();
                throw new ClientSideException($"Coupon isn't active beacause it was expired!");
            }

            if (order.BillingAmount > 0 && order.BillingAmount >= coupon.DiscountAmount)
            {
                order.BillingAmount -= coupon.DiscountAmount;
                order.CouponAmount = coupon.DiscountAmount;
                coupon.IsActive = false;
                _couponRepository.Update(coupon);
                _unitOfWork.CompleteAsync();
            }

            if (order.BillingAmount > 0 && order.BillingAmount < coupon.DiscountAmount)
            {
                order.CouponAmount = order.BillingAmount;
                coupon.IsActive = false;
                _couponRepository.Update(coupon);
                order.BillingAmount = 0;
            }
        }

        private async Task<decimal> CheckEarnPoints(Order order, User user)
        {
            double earnedPoint = 0;

            foreach (var od in order.OrderDetails)
            {
                var product=await _productRepository.GetByIdAsync(od.ProductId);

                if (product == null)
                    throw new NotFoundException($"Product with ProductId ({od.ProductId}) didn't find in the database.");

                if (product.MaxPuanAmount <= Convert.ToDouble(product.Price) * product.EarningPercentage)
                {
                    earnedPoint += (product.MaxPuanAmount * od.Quantity);
                }
                else
                {
                    earnedPoint += (Convert.ToDouble(product.Price) * product.EarningPercentage*od.Quantity);
                }

                if (order.TotalAmount - order.BillingAmount > 0)
                {
                    earnedPoint *= (Convert.ToDouble(order.BillingAmount) / Convert.ToDouble(order.TotalAmount));
                }
            }
            return user.DigitalWalletBalance += Convert.ToDecimal(earnedPoint);
        }

        private void CheckCreditCardUsing()
        {

        }

    }

}
