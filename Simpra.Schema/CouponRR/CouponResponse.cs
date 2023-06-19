using Simpra.Schema.Base;

namespace Simpra.Schema.CouponRR
{
    public class CouponResponse : BaseResponse
    {
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
    }
}
