

namespace Simpra.Core.Entity
{
    public class Coupon : BaseEntity
    {
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

    }
}
