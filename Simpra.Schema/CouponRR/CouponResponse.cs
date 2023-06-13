using Simpra.Core.Entity;
using Simpra.Schema.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Schema.CouponRR
{
    public class CouponResponse:BaseResponse
    {
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
    }
}
