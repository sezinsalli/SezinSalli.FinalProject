using Simpra.Schema.Base;

namespace Simpra.Schema.OrderRR
{
    public class OrderCreateRequest : BaseRequest
    {
        public OrderCreateRequest()
        {
            OrderDetails = new List<OrderDetailRequest>();
        }
        public string CouponCode { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderDetailRequest> OrderDetails { get; set; }

    }
}
