using Simpra.Schema.Base;

namespace Simpra.Schema.ProductRR
{
    public class ProductResponse : BaseResponse
    {
        // Product yanıt modeli
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Property { get; set; }
        public string Definition { get; set; }
        public int Status { get; set; }
        public bool IsActive { get; set; }
        public double EarningPercentage { get; set; }
        public double MaxPuanAmount { get; set; }

    }
}
