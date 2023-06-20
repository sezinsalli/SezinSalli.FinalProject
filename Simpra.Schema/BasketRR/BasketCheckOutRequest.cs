using Simpra.Schema.CreditCardRR;

namespace Simpra.Schema.BasketRR
{
    public class BasketCheckOutRequest
    {
        public string CouponCode { get; set; }
        public CreditCardRequest CreditCard { get; set; }

    }



}
