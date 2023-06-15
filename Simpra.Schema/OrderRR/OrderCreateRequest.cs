using Simpra.Schema.Base;

namespace Simpra.Schema.OrderRR
{
    public class OrderCreateRequest : BaseRequest
    {
        public string CouponCode { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderDetailRequest> OrderDetails { get; set; }

    }
}
