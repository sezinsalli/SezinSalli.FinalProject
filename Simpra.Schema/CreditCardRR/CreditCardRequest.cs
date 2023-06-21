namespace Simpra.Schema.CreditCardRR
{
    public class CreditCardRequest
    {
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }

    }
}
