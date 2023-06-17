using Simpra.Core.Entity;
using Simpra.Core.Repository;

namespace Simpra.Repository.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {

        }


    }
}
