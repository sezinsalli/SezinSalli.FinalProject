using Simpra.Schema.Base;

namespace Simpra.Schema.CategoryRR
{
    public class CategoryResponse : BaseResponse
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
    }
}
