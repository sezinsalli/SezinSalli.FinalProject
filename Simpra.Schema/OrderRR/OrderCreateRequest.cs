using Simpra.Schema.Base;
using Simpra.Schema.CreditCardRR;

namespace Simpra.Schema.OrderRR
{
    public class OrderCreateRequest : BaseRequest
    {
        public OrderCreateRequest()
        {
            OrderDetails = new List<OrderDetailRequest>();
        }
        public string CouponCode { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderDetailRequest> OrderDetails { get; set; }
        public CreditCardRequest CreditCard { get; set; }

    }
}
