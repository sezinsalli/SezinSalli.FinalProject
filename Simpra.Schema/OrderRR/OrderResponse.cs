using Simpra.Core.Entity;
using Simpra.Schema.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Schema.OrderRR
{
    public class OrderResponse:BaseResponse
    {       
        public string CouponCode { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderDetailResponse> OrderDetails { get; set; }
    }
}
