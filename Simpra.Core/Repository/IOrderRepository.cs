using Simpra.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Core.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        List<Order> GetOrdersWithOrderDetails();
    }
}
