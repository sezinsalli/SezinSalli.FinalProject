using Simpra.Core.Entity;
using Simpra.Core.Repository;

namespace Simpra.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {

        }


    }
}
