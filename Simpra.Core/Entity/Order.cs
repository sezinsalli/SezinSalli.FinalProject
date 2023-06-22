using Simpra.Core.Enum;

namespace Simpra.Core.Entity
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }

        public string OrderNumber { get; set; }
        public OrderStatus Status { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal BillingAmount { get; set; }
        public decimal CouponAmount { get; set; }
        public decimal WalletAmount { get; set; }
        public string CouponCode { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
    }
}
