
using BCrypt.Net;
using Simpra.Schema.CreditCardRR;

namespace Simpra.Service.Helper
{
    public static class CreditCardHashHelper
    {
        public static string HashCreditCardInfo(CreditCardRequest creditCard)
        {
            string cardInfo = $"{creditCard.CardNumber}-{creditCard.CVV}-{creditCard.ExpiryYear}-{creditCard.ExpiryMonth}";
            return BCrypt.Net.BCrypt.HashPassword(cardInfo) ;
        }

    }
}
