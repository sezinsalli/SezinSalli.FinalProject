using Simpra.Schema.Base;

namespace Simpra.Schema.OrderRR
{
    public class OrderResponse : BaseResponse
    {
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal BillingAmount { get; set; }
        public decimal CouponAmount { get; set; }
        public decimal WalletAmount { get; set; }
        public string CouponCode { get; set; }
        public string UserId { get; set; }
        public ICollection<OrderDetailResponse> OrderDetails { get; set; }
    }
}
