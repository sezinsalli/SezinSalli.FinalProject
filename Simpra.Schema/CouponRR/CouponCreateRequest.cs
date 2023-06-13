using Simpra.Core.Entity;
using Simpra.Schema.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Schema.CouponRR
{
    public class CouponCreateRequest:BaseRequest
    {
        public int UserId { get; set; }
        public decimal DiscountAmount { get; set; }

        // Kaç gün geçerli olması gerektiğini client'tan alarak datetime'a ilave edeceğiz. Datetime'da alabilirdik ama gün olarak almak client için daha rahat olabilir.
        public int ExpirationDay { get; set; }

    }
}
