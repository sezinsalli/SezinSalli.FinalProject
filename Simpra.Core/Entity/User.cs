

namespace Simpra.Core.Entity
{
    public class User:BaseEntity
    {        
        public string UserName { get; set; }
        public Coupon Coupon { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
