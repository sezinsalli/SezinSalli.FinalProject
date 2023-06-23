using Simpra.Schema.Base;

namespace Simpra.Schema.UserRR
{
    public class AppUserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal DigitalWalletBalance { get; set; }
        public string DigitalWalletInformation { get; set; }

    }
}
