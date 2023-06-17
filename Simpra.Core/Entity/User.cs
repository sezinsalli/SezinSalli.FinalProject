

namespace Simpra.Core.Entity
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal DigitalWalletBalance { get; set; }
        public string DigitalWalletInformation { get; set; }
        public ICollection<Coupon> Coupon { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
