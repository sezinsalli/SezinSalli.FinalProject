using Simpra.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Schema.OrderRR
{
    public class OrderUpdateRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CouponCode { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderDetailRequest> OrderDetails { get; set; }
    }
}
