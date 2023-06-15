using Simpra.Core.Entity;

namespace Simpra.Core.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        List<Order> GetOrdersWithOrderDetails();
    }
}
