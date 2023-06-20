

namespace Simpra.Schema.BasketRR
{
    public class BasketRequest
    {
        public List<BasketItemRequest> BasketItems { get; set; }
        public decimal TotalPrice
        {
            get => BasketItems.Sum(x => x.UnitPrice * x.Quantity);
        }
    }
}
