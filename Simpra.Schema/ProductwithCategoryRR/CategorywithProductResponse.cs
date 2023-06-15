using Simpra.Schema.CategoryRR;
using Simpra.Schema.ProductRR;

namespace Simpra.Schema.ProductwithCategoryRR
{
    public class CategorywithProductResponse : CategoryResponse
    {

        public List<ProductResponse> Products { get; set; }
    }
}
