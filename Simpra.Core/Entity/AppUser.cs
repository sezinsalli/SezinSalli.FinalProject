

using Microsoft.AspNetCore.Identity;

namespace Simpra.Core.Entity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal DigitalWalletBalance { get; set; }
        public string DigitalWalletInformation { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public ICollection<Coupon> Coupon { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
