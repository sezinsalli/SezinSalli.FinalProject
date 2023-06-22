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

        // ====> Arrange <====
        // User
        private readonly List<AppUser> _users = new List<AppUser>()
        {
            new AppUser
            {
                Id = "user-test-id-1",
                UserName = "user-test-1",
                DigitalWalletBalance = 40
            },
            new AppUser
            {
                Id = "user-test-id-2",
                UserName = "user-test-2",
                DigitalWalletBalance = 240
            }
        };

        // Coupon
        private readonly List<Coupon> _coupons = new List<Coupon>()
        {
            new Coupon
            {
                Id = 1,
                UserId= "user-test-id-1",
                CouponCode = "A1D2F3G4H5",
                DiscountAmount = 60,
                ExpirationDate = DateTime.Now.AddDays(10),
                IsActive=true,

            },
            new Coupon
            {
                Id = 2,
                UserId= "user-test-id-2",
                CouponCode = "A5D4F3G2H1",
                DiscountAmount = 160,
                ExpirationDate = DateTime.Now.AddDays(10),
                IsActive=true,
            }
        };

        // Products
        private readonly List<Product> _products = new List<Product>()
        {
                new Product
                {
                    Id = 1,
                    Price = 100,
                    EarningPercentage = 0.12,
                    MaxPuanAmount = 10,
                    Stock = 20
                },
                new Product
                {
                    Id = 2,
                    Price = 200,
                    EarningPercentage = 0.06,
                    MaxPuanAmount = 12,
                    Stock = 8
                },
                new Product
                {
                    Id = 3,
                    Price = 120,
                    EarningPercentage = 0.05,
                    MaxPuanAmount = 4,
                    Stock = 24
                }
        };

        [Fact]
        public async Task CreateOrderAsync_CheckAllCalculateTest1_Success()
        {
            // ====> Arrange <====
            _userServiceMock.Setup(repo => repo.GetById(It.IsAny<string>()))
                .Returns((string id) => _users.FirstOrDefault(u => u.Id == id));

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => _products.FirstOrDefault(p => p.Id == id));

            _couponRepositoryMock.Setup(repo => repo.Where(It.IsAny<Expression<Func<Coupon, bool>>>()))
                .Returns<Expression<Func<Coupon, bool>>>(expression => _coupons.AsQueryable().Where(expression.Compile())
                .AsQueryable());

            // Order
            var order = new Order
            {
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { ProductId = 1, Quantity = 3, UnitPrice = 100 },
                },
                CouponCode = _coupons[0].CouponCode
            };

            // ====> Act <====
            var result = await _orderService.CreateOrderAsync(order, _users[0].Id, "hashed-creditcard");

            // ====> Assert <====
            // 300 TL sipariş => 
            Assert.Equal(300, result.TotalAmount);
            Assert.Equal((result.CouponAmount + result.WalletAmount + result.BillingAmount), result.TotalAmount);

            // 200 TL fatura => 
            Assert.Equal(200, result.BillingAmount);

            // 40 TL puan kullanımı => 
            Assert.Equal(40, result.WalletAmount);

            // 60 TL kupon kullanımı => 
            Assert.Equal(_coupons[0].DiscountAmount, result.CouponAmount);
            Assert.Equal(60, result.CouponAmount);
            Assert.False(_coupons[0].IsActive);

            // 300 TL siparişin 100 TL si indirim => (100*0,12*0,6666*3)= 24 TL puan kazanmalı
            Assert.Equal(24.0m, Math.Round(_users[0].DigitalWalletBalance,1));

            // Product stok => 17
            Assert.Equal(17, _products[0].Stock);

            Assert.Equal(OrderStatus.Pending, order.Status);
            Assert.True(order.IsActive);
        }

        [Fact]
        public async Task CreateOrderAsync_CheckAllCalculateTest2_Success()
        {
            // ====> Arrange <====
            _userServiceMock.Setup(repo => repo.GetById(It.IsAny<string>()))
                .Returns((string id) => _users.FirstOrDefault(u => u.Id == id));

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => _products.FirstOrDefault(p => p.Id == id));

            _couponRepositoryMock.Setup(repo => repo.Where(It.IsAny<Expression<Func<Coupon, bool>>>()))
                .Returns<Expression<Func<Coupon, bool>>>(expression => _coupons.AsQueryable().Where(expression.Compile())
                .AsQueryable());

            // Order
            var order = new Order
            {
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { ProductId = 1, Quantity = 6, UnitPrice = 100 },
                    new OrderDetail { ProductId = 2, Quantity = 2, UnitPrice = 200 },
                },
                CouponCode = _coupons[0].CouponCode
            };

            // ====> Act <====
            var result = await _orderService.CreateOrderAsync(order, _users[0].Id, "hashed-creditcard");

            // ====> Assert <====
            // 1000 TL sipariş => 
            Assert.Equal(1000, result.TotalAmount);
            Assert.Equal((result.CouponAmount + result.WalletAmount + result.BillingAmount), result.TotalAmount);

            // 900 TL fatura => 
            Assert.Equal(900, result.BillingAmount);

            // 40 TL puan kullanımı => 
            Assert.Equal(40, result.WalletAmount);

            // 60 TL kupon kullanımı => 
            Assert.Equal(_coupons[0].DiscountAmount, result.CouponAmount);
            Assert.Equal(60, result.CouponAmount);
            Assert.False(_coupons[0].IsActive);

            // 1000 TL siparişin 100 TL si indirim => Product 1 (10 TL x 6 adet) + Product 2 (10,8 TL x 2 adet) = 81.6 TL
            Assert.Equal(81.6m, Math.Round(_users[0].DigitalWalletBalance,1));

            // Product[0] stok => 14
            // Product[1] stok => 6
            Assert.Equal(14, _products[0].Stock);
            Assert.Equal(6, _products[1].Stock);

            Assert.Equal(OrderStatus.Pending, order.Status);
            Assert.True(order.IsActive);
        }

        [Fact]
        public async Task CreateOrderAsync_CheckAllCalculateTest3_Success()
        {
            // ====> Arrange <====
            _userServiceMock.Setup(repo => repo.GetById(It.IsAny<string>()))
                .Returns((string id) => _users.FirstOrDefault(u => u.Id == id));

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => _products.FirstOrDefault(p => p.Id == id));

            _couponRepositoryMock.Setup(repo => repo.Where(It.IsAny<Expression<Func<Coupon, bool>>>()))
                .Returns<Expression<Func<Coupon, bool>>>(expression => _coupons.AsQueryable().Where(expression.Compile())
                .AsQueryable());

            // Order
            var order = new Order
            {
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { ProductId = 1, Quantity = 6, UnitPrice = 100 },
                    new OrderDetail { ProductId = 2, Quantity = 5, UnitPrice = 200 },
                    new OrderDetail { ProductId = 3, Quantity = 4, UnitPrice = 120 },
                },
                CouponCode = _coupons[1].CouponCode
            };

            // ====> Act <====
            var result = await _orderService.CreateOrderAsync(order, _users[1].Id, "hashed-creditcard");

            // ====> Assert <====
            // 2080 TL sipariş => 
            Assert.Equal(2080, result.TotalAmount);
            Assert.Equal((result.CouponAmount + result.WalletAmount + result.BillingAmount), result.TotalAmount);

            // 1680 TL fatura => 
            Assert.Equal(1680, result.BillingAmount);

            // 240 TL puan kullanımı => 
            Assert.Equal(240, result.WalletAmount);

            // 160 TL kupon kullanımı => 
            Assert.Equal(_coupons[1].DiscountAmount, result.CouponAmount);
            Assert.Equal(160, result.CouponAmount);
            Assert.False(_coupons[1].IsActive);

            // 2080 TL siparişin 400 TL si indirim => 0,8076 (totalamount/billingamount)
            // Product 1 (9,69 TL x 6 adet) =58,15 TL
            // Product 2 (9,69 TL x 5 adet) = 48,45 TL
            // Product 3 (4 TL x 4 adet) = 16 TL
            Assert.Equal(122.6m, Math.Round(_users[1].DigitalWalletBalance,1));

            // Product Stok
            Assert.Equal(14, _products[0].Stock);
            Assert.Equal(3, _products[1].Stock);
            Assert.Equal(20, _products[2].Stock);

            Assert.Equal(OrderStatus.Pending, order.Status);
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
