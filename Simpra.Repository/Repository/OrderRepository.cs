using Simpra.Core.Entity;
using Simpra.Core.Repository;
using System.Data.Entity;

namespace Simpra.Repository.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {

        }

        public List<Order> GetOrdersWithOrderDetails()
        {
            // TODO: Sonradan incelenecek asenkron yapılamadı!
            var orders = _context.Orders.ToList();

            foreach (var order in orders)
            {
                order.OrderDetails = _context.OrderDetails
                    .Where(od => od.OrderId == order.Id)
                    .ToList();
            }

            return orders;

        }

    }
}
