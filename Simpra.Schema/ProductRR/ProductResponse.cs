using Simpra.Schema.Base;
using Simpra.Schema.CategoryRR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Schema.ProductRR
{
    public class ProductResponse : BaseResponse
    {
        // Product yanıt modeli
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Property { get; set; }
        public string Definition { get; set; }
        public bool IsActive { get; set; }
        public double EarningPercentage { get; set; }
        public double MaxPuanAmount { get; set; }
        public ICollection<CategoryResponse> Categories { get; set; }
    }
}
