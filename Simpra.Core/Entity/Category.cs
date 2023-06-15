namespace Simpra.Core.Entity
{

    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
