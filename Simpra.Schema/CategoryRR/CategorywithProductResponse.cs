using Simpra.Schema.ProductRR;

namespace Simpra.Schema.CategoryRR
{
    public class CategoryWithProductResponse : CategoryResponse
    {

        public List<ProductResponse> Products { get; set; }
    }
}
