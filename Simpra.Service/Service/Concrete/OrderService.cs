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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderService(IGenericRepository<Order> repository, IUnitOfWork unitofWork, IOrderRepository orderRepository, IMapper mapper,IGenericRepository<User> userRepository) : base(repository, unitofWork)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _unitOfWork= unitofWork;
            _userRepository = userRepository;
        }

        public List<Order> GetOrdersWithOrderDetails()
        {
            return _orderRepository.GetOrdersWithOrderDetails();

        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Get user
            var user= await _userRepository.GetByIdAsync(order.UserId);

            if (user == null)
            {
                throw new NotFoundException($"Order with userId ({order.UserId}) didn't find in the database.");
            }

            // Calculate total price
            order.TotalPrice=order.OrderDetails.Sum(x => x.Quantity * x.UnitPrice);

            var totalPrice = order.TotalPrice;

            // CheckPointsBalance
            CheckPointsBalanceAsync(ref user, ref totalPrice);

            // CheckDigitalWalletBalance
            CheckDigitalWalletBalance(ref user, ref totalPrice);

            // CheckCouponUsing

            // CheckCreditCartUsing
            // Kredi Kartı Kullanımı
            if (totalPrice > 0)
            {

            }

            order.IsActive = true;
            _userRepository.Update(user);
            await _orderRepository.AddAsync(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }


        public void CheckPointsBalanceAsync(ref User user,ref decimal totalPrice)
        {
            // Puan kullanımı
            if (user.PointsBalance > 0 && totalPrice >= user.PointsBalance)
            {
                totalPrice = totalPrice - user.PointsBalance;
                user.PointsBalance = 0;
            }

            if (user.PointsBalance > 0 && totalPrice < user.PointsBalance)
            {
                user.PointsBalance = user.PointsBalance - totalPrice;
                totalPrice = 0;
            }

            return;
        }

        public void CheckDigitalWalletBalance(ref User user,ref decimal totalPrice)
        {

            // Dijital Cüzdan Kullanımı
            if (totalPrice > 0 && totalPrice >= user.DigitalWalletBalance)
            {
                totalPrice = totalPrice - user.DigitalWalletBalance;
                user.DigitalWalletBalance = 0;
            }

            if (totalPrice > 0 && totalPrice < user.DigitalWalletBalance)
            {
                user.DigitalWalletBalance = user.DigitalWalletBalance - totalPrice;
                totalPrice = 0;
            }
        }




    }

}
