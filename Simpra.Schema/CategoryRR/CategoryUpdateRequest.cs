using Simpra.Schema.Base;

namespace Simpra.Schema.CategoryRR
{
    public class CategoryUpdateRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
    }
}
