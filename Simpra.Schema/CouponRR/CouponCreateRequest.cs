using Simpra.Schema.Base;

namespace Simpra.Schema.CouponRR
{
    public class CouponCreateRequest : BaseRequest
    {
        public string UserId { get; set; }
        public decimal DiscountAmount { get; set; }

        // Kaç gün geçerli olması gerektiğini client'tan alarak datetime'a ilave edeceğiz. Datetime'da alabilirdik ama gün olarak almak client için daha rahat olabilir.
        public int ExpirationDay { get; set; }

    }
}
