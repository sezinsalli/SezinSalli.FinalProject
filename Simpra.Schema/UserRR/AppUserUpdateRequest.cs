namespace Simpra.Schema.UserRR
{
    public class AppUserUpdateRequest
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal DigitalWalletBalance { get; set; }
        public string DigitalWalletInformation { get; set; }
    }
}
