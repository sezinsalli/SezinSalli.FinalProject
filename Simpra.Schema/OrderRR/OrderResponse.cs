using Simpra.Schema.Base;

namespace Simpra.Schema.OrderRR
{
    public class OrderResponse : BaseResponse
    {
        public string CouponCode { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderDetailResponse> OrderDetails { get; set; }
    }
}
