
using Simpra.Core.Enum;

namespace Simpra.Core.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Property { get; set; }
        public string Definition { get; set; }
        public ProductStatus Status { get; set; }
        public bool IsActive { get; set; }
        public double EarningPercentage { get; set; }
        public double MaxPuanAmount { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
