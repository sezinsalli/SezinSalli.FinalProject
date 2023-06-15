namespace Simpra.Schema.Basket
{
    public class BasketCheckOutRequest
    {
        public string CouponCode { get; set; }
        public int UserId { get; set; }
        public List<BasketItemRequest> BasketItems { get; set; }
        public decimal TotalPrice
        {
            get => BasketItems.Sum(x => x.UnitPrice * x.Quantity);
        }

    }
}
