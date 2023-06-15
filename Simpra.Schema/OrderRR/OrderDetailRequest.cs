using Simpra.Schema.Base;

namespace Simpra.Schema.OrderRR
{
    public class OrderDetailRequest : BaseRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
