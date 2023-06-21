using Simpra.Schema.CreditCardRR;

namespace Simpra.Api.Helper
{
    public static class CreditCardHashHelper
    {
        public static string HashCreditCardInfo(CreditCardRequest creditCard)
        {
            string cardInfo = $"{creditCard.CardNumber}-{creditCard.CVV}-{creditCard.ExpiryYear}-{creditCard.ExpiryMonth}";
            return BCrypt.Net.BCrypt.HashPassword(cardInfo);
        }

    }
}
