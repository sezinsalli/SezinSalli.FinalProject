using Simpra.Schema.Base;

namespace Simpra.Schema.CategoryRR
{
    public class CategoryCreateRequest : BaseRequest
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
    }
}
