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

        public List<Order> GetOrdersWithOrderDetails()
        {
            return _orderRepository.GetOrdersWithOrderDetails();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
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
            CheckDigitalWalletBalance(ref user, ref order);

            // Kredi Kartı Kullanımı
            if (order.BillingAmount > 0)
            {

            }

            // Puan Kazanma
            var earnedPoints = EarnPoints(order, user);
            user.DigitalWalletBalance = user.DigitalWalletBalance + earnedPoints;

            order.IsActive = true;
            _userRepository.Update(user);
            await _orderRepository.AddAsync(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }

        public void CheckDigitalWalletBalance(ref User user, ref Order order)
        {
            // Dijital Cüzdan Kullanımı
            if (order.BillingAmount > 0 && order.BillingAmount >= user.DigitalWalletBalance)
            {
                order.BillingAmount = order.BillingAmount - user.DigitalWalletBalance;
                order.WalletAmount = user.DigitalWalletBalance;
                user.DigitalWalletBalance = 0;
            }

            if (order.BillingAmount > 0 && order.BillingAmount < user.DigitalWalletBalance)
            {
                user.DigitalWalletBalance = user.DigitalWalletBalance - order.BillingAmount;
                order.WalletAmount = order.BillingAmount;
                order.BillingAmount = 0;
            }
        }

        public void CheckCouponUsing(ref Coupon coupon, ref Order order)
        {
            if (coupon.UserId != order.UserId)
            {
                throw new ClientSideException($"Coupon and User Id doesn't match!");
            }

            if (coupon.IsActive != true)
            {
                throw new ClientSideException($"Coupon isn't active!");
            }


            if (coupon.ExpirationDate < DateTime.Now)
            {
                coupon.IsActive = false;
                _couponRepository.Update(coupon);
                _unitOfWork.CompleteAsync();
                throw new ClientSideException($"Coupon isn't active beacause it was expired!");
            }

            // Coupon Kullanımı
            if (order.BillingAmount > 0 && order.BillingAmount >= coupon.DiscountAmount)
            {
                order.BillingAmount = order.BillingAmount - coupon.DiscountAmount;
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

        public decimal EarnPoints(Order order, User user)
        {
            double earnedPoint = 0;

            foreach (var od in order.OrderDetails)
            {
                //var product= _productRepository.GetByIdAsync(od.ProductId).Result;

                var product = new Product { Id = 1, CategoryId = 1, IsActive = true, Price = 100, MaxPuanAmount = 10, EarningPercentage = 0.12 };

                if (product == null)
                {
                    throw new NotFoundException($"Product with ProductId ({od.ProductId}) didn't find in the database.");
                }

                if (product.MaxPuanAmount <= Convert.ToDouble(product.Price) * product.EarningPercentage)
                {
                    earnedPoint = earnedPoint + product.MaxPuanAmount * od.Quantity;
                }
                else
                {
                    earnedPoint = earnedPoint + Convert.ToDouble(product.Price) * product.EarningPercentage;
                }

                if (order.TotalAmount - order.BillingAmount > 0)
                {
                    earnedPoint = earnedPoint * (Convert.ToDouble(order.BillingAmount) / Convert.ToDouble(order.TotalAmount));
                }
            }

            return user.DigitalWalletBalance = user.DigitalWalletBalance + Convert.ToDecimal(earnedPoint);
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
            CheckDigitalWalletBalance(ref user, ref order);

            // Kredi Kartı Kullanımı
            if (order.BillingAmount > 0)
            {

            }

            // Puan Kazanma
            var earnedPoints = EarnPoints(order, user);
            user.DigitalWalletBalance = user.DigitalWalletBalance + earnedPoints;

            order.IsActive = true;
            _userRepository.Update(user);
            await _orderRepository.AddAsync(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }

    }

}
