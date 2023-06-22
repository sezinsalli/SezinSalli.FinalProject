using Microsoft.EntityFrameworkCore;
using Moq;
using Simpra.Core.Entity;
using Simpra.Core.Enum;
using Simpra.Core.Repository;
using Simpra.Core.Service;
using Simpra.Core.UnitofWork;
using Simpra.Service.Exceptions;
using Simpra.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Test
{
    public class OrderServiceTest
    {
        private readonly OrderService _orderService;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ICouponRepository> _couponRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public OrderServiceTest()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _userServiceMock = new Mock<IUserService>();
            _couponRepositoryMock = new Mock<ICouponRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _orderService = new OrderService(
                _unitOfWorkMock.Object,
                _orderRepositoryMock.Object,
                _userServiceMock.Object,
                _couponRepositoryMock.Object,
                _productRepositoryMock.Object
            );
        }

        [Fact]
        public async Task CreateOrderAsync_CheckAllCalculateTest1_Success()
        {
            // ====> Arrange <====

            // User
            var user = new AppUser
            {
                Id = "user-test-id",
                UserName = "user-test",
                DigitalWalletBalance = 50
            };
            _userServiceMock
                .Setup(repo => repo.GetById(user.Id))
                .Returns(user);

            // Product
            var product = new Product
            {
                Id = 1,
                Price = 100,
                EarningPercentage = 0.12,
                MaxPuanAmount = 10,
                Stock = 20
            };
            _productRepositoryMock
                .Setup(repo => repo.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            // Coupon
            var coupons = new List<Coupon>()
            {
                new Coupon
                {
                    Id = 1,
                    UserId= user.Id,
                    CouponCode = "A1D2F3G4H5",
                    DiscountAmount = 100,
                    ExpirationDate = DateTime.Now.AddDays(10),
                    IsActive=true,
                }
            };
            _couponRepositoryMock
                .Setup(repo => repo.Where(It.IsAny<Expression<Func<Coupon, bool>>>()))
                .Returns<Expression<Func<Coupon, bool>>>(expression => coupons.AsQueryable().Where(expression.Compile())
                .AsQueryable());

            // Order
            var order = new Order
            {
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { ProductId = 1, Quantity = 3, UnitPrice = 100 },
                },
                CouponCode = "A1D2F3G4H5"
            };

            // ====> Act <====
            var result = await _orderService.CreateOrderAsync(order, user.Id, "hashed-creditcard");

            // ====> Assert <====
            // 300 TL sipariş => 
            Assert.Equal(300, result.TotalAmount);
            Assert.Equal((result.CouponAmount+result.WalletAmount+result.BillingAmount), result.TotalAmount);

            // 150 TL fatura => 
            Assert.Equal(150, result.BillingAmount);

            // 50 TL puan kullanımı => 
            Assert.Equal(50, result.WalletAmount);

            // 100 TL kupon kullanımı => 
            Assert.Equal(coupons.First().DiscountAmount, result.CouponAmount);
            Assert.Equal(100, result.CouponAmount);
            Assert.False(coupons.First().IsActive);

            // 300 TL siparişin 150 TL si indirim => (100*0,12*0,5*3)= 18 TL puan kazanmalı
            Assert.Equal(18, user.DigitalWalletBalance);

            Assert.Equal(OrderStatus.Pending,order.Status);
            Assert.True(order.IsActive);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOrders()
        {
            // Arrange
            var orders = new List<Order> { new Order(), new Order() };
            _orderRepositoryMock.Setup(repo => repo.GetAllWithIncludeAsync("OrderDetails")).ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(orders, result);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ShouldReturnOrder()
        {
            // Arrange
            int orderId = 1;
            var order = new Order { Id = orderId };
            _orderRepositoryMock.Setup(repo => repo.GetByIdWithIncludeAsync(orderId, "OrderDetails")).ReturnsAsync(order);

            // Act
            var result = await _orderService.GetByIdAsync(orderId);

            // Assert
            Assert.Equal(order, result);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ShouldThrowNotFoundException()
        {
            // Arrange
            int orderId = 1;
            _orderRepositoryMock.Setup(repo => repo.GetByIdWithIncludeAsync(orderId, "OrderDetails")).ReturnsAsync((Order)null);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _orderService.GetByIdAsync(orderId));
        }

        [Fact]
        public async Task UpdateOrderStatusAsync_ExistingId_ShouldUpdateOrderStatusAndReturnOrder()
        {
            // Arrange
            int orderId = 1;
            var order = new Order { Id = orderId, Status = OrderStatus.Pending };
            _orderRepositoryMock.Setup(repo => repo.GetByIdWithIncludeAsync(orderId, "OrderDetails")).ReturnsAsync(order);

            // Act
            var result = await _orderService.UpdateOrderStatusAsync(orderId, (int)OrderStatus.Shipped, "user");

            // Assert
            Assert.Equal(OrderStatus.Shipped, result.Status);
            Assert.Equal("user", result.UpdatedBy);
            _orderRepositoryMock.Verify(repo => repo.Update(order), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderStatusAsync_NonExistingId_ShouldThrowNotFoundException()
        {
            // Arrange
            int orderId = 1;
            _orderRepositoryMock.Setup(repo => repo.GetByIdWithIncludeAsync(orderId, "OrderDetails")).ReturnsAsync((Order)null);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _orderService.UpdateOrderStatusAsync(orderId, (int)OrderStatus.Shipped, "user"));
        }

    }
}
