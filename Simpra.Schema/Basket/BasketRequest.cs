

namespace Simpra.Schema.Basket
{
    public class BasketRequest
    {
        public int UserId { get; set; }
        public List<BasketItemRequest> BasketItems { get; set; }
        public decimal TotalPrice
        {
            get => BasketItems.Sum(x => x.UnitPrice * x.Quantity);
        }
    }
}
