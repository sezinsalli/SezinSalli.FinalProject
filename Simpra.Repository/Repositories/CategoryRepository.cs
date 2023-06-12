using Microsoft.EntityFrameworkCore;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Repository.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<List<Category>> GetProductsByCategoryId(int categoryId)
        {
            return await _appDbContext.Categories
        .Where(c => c.Id == categoryId)
        .Include(c => c.ProductCategories)
        .ThenInclude(pc => pc.Product)
        .ToListAsync();
        }
    }
}
