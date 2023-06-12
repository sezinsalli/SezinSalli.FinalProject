using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Core.Entity
{
    public class Order : BaseEntity
    {
        //Sipariş (sepet tutarı, kupon tutarı, kupon kodu, puan tutarı, vs.) 
        
        public string CouponCode { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Coupon Coupon { get; set; }

        //public decimal PointAmount { get; set; }
        //order ıtemlarla donerek total prıce'ı olusturacagım.
        //public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);
        //public decimal DiscountAmount { get; set; }
    }
}
