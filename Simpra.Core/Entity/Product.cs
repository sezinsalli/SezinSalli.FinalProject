using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Core.Entity
{
    [Table("Product", Schema = "dbo")]
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Property { get; set; }
        public string Definition { get; set; }
        public bool isActive { get; set; }
        public double EarningPercentage { get; set; }
        public double MaxPuanAmount { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
