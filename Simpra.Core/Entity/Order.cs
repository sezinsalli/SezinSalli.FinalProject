using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Core.Entity
{
    public class Order : BaseEntity
    {
        public decimal TotalAmount { get; set; }
        public decimal CouponAmount { get; set; }
        public string CouponCode { get; set; }
        public decimal PointAmount { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
