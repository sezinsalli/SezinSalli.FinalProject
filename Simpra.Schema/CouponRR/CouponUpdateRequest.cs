using Simpra.Schema.Base;
using Simpra.Schema.UserRR;

namespace Simpra.Schema.CouponRR
{
    public class CouponUpdateRequest : BaseRequest
    {
        public int Id { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public UserRequest User { get; set; }
    }
}
