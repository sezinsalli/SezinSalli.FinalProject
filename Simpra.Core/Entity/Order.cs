using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Core.Entity
{
    public class Order : BaseEntity
    {               
        public bool IsActive { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal BillingAmount { get; set; }
        public decimal CouponAmount { get; set; }
        public decimal WalletAmount { get; set; }
        public string CouponCode { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
