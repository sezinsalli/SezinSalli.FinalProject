using Microsoft.EntityFrameworkCore;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Repository.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {

        }

        public List<Order> GetOrdersWithOrderDetails()
        {
            // TODO: Sonradan incelenecek asenkron yapılamadı!
            var orders = _appDbContext.Orders.ToList();

            foreach (var order in orders)
            {
                order.OrderDetails = _appDbContext.OrderDetails
                    .Where(od => od.OrderId == order.Id)
                    .ToList();
            }

            return orders;

        }

    }
}
