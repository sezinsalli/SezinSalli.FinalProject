﻿using Simpra.Core.Entity;
using Simpra.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Service.Service.Abstract
{
    public interface IOrderService : IService<Order>
    {
        List<Order> GetOrdersWithOrderDetails();
        Task<Order> CreateOrderAsync(Order order);
        void CheckPointsBalanceAsync(ref User user, ref decimal totalPrice);
        void CheckDigitalWalletBalance(ref User user, ref decimal totalPrice);
    }
}
