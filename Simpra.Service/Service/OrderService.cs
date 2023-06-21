﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Simpra.Core.Entity;
using Simpra.Core.Enum;
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
        private readonly IUserService _userService;
        private readonly ICouponRepository _couponRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitofWork, IOrderRepository orderRepository, IUserService userService, ICouponRepository couponRepository, IProductRepository productRepository) : base(orderRepository, unitofWork)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _unitOfWork = unitofWork ?? throw new ArgumentNullException(nameof(unitofWork));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
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
                Log.Error(ex, "GetAllAsync Exception");
                throw new Exception($"Something went wrong! Error message:{ex.Message}");
            }
        }
        public override async Task<Order> GetByIdAsync(int id)
        {
            try
            {
                var order = await _orderRepository.GetByIdWithIncludeAsync(id, "OrderDetails");
                if (order == null)
                    throw new NotFoundException($"Order ({id}) not found!");

                return order;
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
        public async Task<Order> UpdateOrderStatusAsync(int id, int status)
        {
            try
            {
                var order = await _orderRepository.GetByIdWithIncludeAsync(id, "OrderDetails");

                if (order == null)
                    throw new NotFoundException($"Order ({id}) not found!");

                order.Status=SetOrderStatus(status);
                _orderRepository.Update(order);
                await _unitOfWork.CompleteAsync();
                return order;
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    Log.Warning(ex, "UpdateOrderStatusAsync Exception - Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                }
                Log.Error(ex, "UpdateOrderStatusAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }
        public async Task<Order> CreateOrderAsync(Order order,string userId)
        {
            try
            {
                var user = _userService.GetById(userId);
                if (user == null)
                    throw new NotFoundException($"Order with userId ({userId}) didn't find in the database.");

                order.TotalAmount = order.OrderDetails.Sum(x => x.Quantity * x.UnitPrice);
                order.BillingAmount = order.TotalAmount;
                order.UserId= user.Id;
                await CheckAndUpdateProductStock(order);

                if (!string.IsNullOrEmpty(order.CouponCode))
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

                CheckDigitalWalletUsing(ref user, ref order);

                if (order.BillingAmount > 0)
                {
                    CheckCreditCardUsing();
                }

                user.DigitalWalletBalance = await CheckEarnPoints(order, user);
                order.IsActive = true;
                order.Status = Core.Enum.OrderStatus.Pending;
                order.OrderNumber = await GenerateOrderNumber();
                await _userService.UpdateWalletBalanceAsync(user.DigitalWalletBalance, user.Id);
                await _orderRepository.AddAsync(order);
                await _unitOfWork.CompleteAsync();
                return order;
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    Log.Warning(ex, "CreateOrderAsync Exception - Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                }
                Log.Error(ex, "CreateOrderAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }

        private async Task CheckAndUpdateProductStock(Order order)
        {
            foreach (var od in order.OrderDetails)
            {
                var product =await _productRepository.GetByIdAsync(od.ProductId);
                if (product == null)
                    throw new NotFoundException($"Product with ProductId ({od.ProductId}) didn't find in the database.");

                product.Stock -= od.Quantity;

                if (product.Stock < 0)
                    throw new NotFoundException($"Product with ProductId ({od.ProductId}) is not enough in stock.");

                _productRepository.Update(product);
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
            }

            if (order.BillingAmount > 0 && order.BillingAmount < coupon.DiscountAmount)
            {
                order.CouponAmount = order.BillingAmount;
                coupon.IsActive = false;
                _couponRepository.Update(coupon);
                order.BillingAmount = 0;
            }
        }
        private void CheckDigitalWalletUsing(ref AppUser user, ref Order order)
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
        private void CheckCreditCardUsing()
        {

        }
        private async Task<decimal> CheckEarnPoints(Order order, AppUser user)
        {
            double earnedPoint = 0;
            double discountRate = Convert.ToDouble(order.BillingAmount/order.TotalAmount);

            if (discountRate == 0)
                return user.DigitalWalletBalance;

            foreach (var od in order.OrderDetails)
            {
                var product = await _productRepository.GetByIdAsync(od.ProductId);
                if (product == null)
                    throw new NotFoundException($"Product with ProductId ({od.ProductId}) didn't find in the database.");

                if (product.MaxPuanAmount <= Convert.ToDouble(product.Price) * product.EarningPercentage * discountRate)
                {
                    earnedPoint += (product.MaxPuanAmount * od.Quantity);
                }
                else
                {
                    earnedPoint += (Convert.ToDouble(product.Price) * product.EarningPercentage * od.Quantity * discountRate);
                }
            }
            return user.DigitalWalletBalance += Convert.ToDecimal(earnedPoint);
        }
        private async Task<string> GenerateOrderNumber()
        {
            var ordernumber = 0;
            do
            {
                Random random = new Random();
                ordernumber = random.Next(100000000, 999999999);
            } while (await _orderRepository.AnyAsync(x => x.OrderNumber == ordernumber.ToString()));

            return ordernumber.ToString();
        }
        private OrderStatus SetOrderStatus(int status)
        {
            switch (status)
            {
                case 1:
                    return OrderStatus.Pending;
                case 2:
                    return OrderStatus.Processing;
                case 3:
                    return OrderStatus.Shipped;
                case 4:
                    return OrderStatus.Delivered;
                case 5:
                    return OrderStatus.Cancelled;
                case 6:
                    return OrderStatus.Returned;
                case 7:
                    return OrderStatus.OnHold;
                case 8:
                    return OrderStatus.None;
                default:
                    throw new ClientSideException("Invalid order status!");
            }
        }
    }

}
