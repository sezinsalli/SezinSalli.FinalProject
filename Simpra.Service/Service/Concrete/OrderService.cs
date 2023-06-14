using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.UnitofWork;
using Simpra.Repository.Repositories;
using Simpra.Schema.OrderRR;
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
    public class OrderService : Service<Order>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderService(IGenericRepository<Order> repository, IUnitOfWork unitofWork, IOrderRepository orderRepository, IMapper mapper,IGenericRepository<User> userRepository, ICouponRepository couponRepository) : base(repository, unitofWork)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _unitOfWork = unitofWork;
            _userRepository = userRepository;
            _couponRepository = couponRepository;
        }

        public List<Order> GetOrdersWithOrderDetails()
        {
            return _orderRepository.GetOrdersWithOrderDetails();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Check user
            var user= await _userRepository.GetByIdAsync(order.UserId);

            if (user == null)
            {
                throw new NotFoundException($"Order with userId ({order.UserId}) didn't find in the database.");
            }

            // Calculate total price
            order.TotalAmount =order.OrderDetails.Sum(x => x.Quantity * x.UnitPrice);
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

            // Digital Wallet Using
            CheckDigitalWalletBalance(ref user, ref order);

            // CheckCreditCartUsing
            // Kredi Kartı Kullanımı
            if (order.BillingAmount > 0)
            {

            }

            order.IsActive = true;
            _userRepository.Update(user);
            await _orderRepository.AddAsync(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }

        public void CheckDigitalWalletBalance(ref User user,ref Order order)
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
                order.WalletAmount= order.BillingAmount;
                order.BillingAmount = 0;
            }
        }

        public void CheckCouponUsing (ref Coupon coupon,ref Order order)
        {
            if (coupon.UserId != order.UserId)
            {
                throw new ClientSideException($"Coupon and User Id doesn't match!");
            }

            if (coupon.IsActive !=true)
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
    }

}
