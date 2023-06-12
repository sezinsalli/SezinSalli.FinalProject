using Simpra.Schema.CategoryRR;
using Simpra.Schema.ProductRR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Schema.ProductwithCategoryRR
{
    public class CategorywithProductResponse : CategoryResponse
    {

        public List<ProductResponse> Products { get; set; }
    }
}
