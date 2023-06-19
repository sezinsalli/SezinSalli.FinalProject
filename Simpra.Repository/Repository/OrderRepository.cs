using Simpra.Core.Entity;
using Simpra.Core.Repository;

namespace Simpra.Repository.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {

        }

    }
}
