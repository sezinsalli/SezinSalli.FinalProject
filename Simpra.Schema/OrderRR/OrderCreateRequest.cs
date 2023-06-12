using Simpra.Core.Entity;
using Simpra.Schema.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Schema.OrderRR
{
    public class OrderCreateRequest:BaseRequest
    {
        public string CouponCode { get; set; }       
        public decimal TotalPrice { get; set; }       
        public ICollection<OrderDetailRequest> OrderDetails { get; set; }
       
    }
}
