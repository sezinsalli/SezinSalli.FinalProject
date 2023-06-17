using Simpra.Core.Entity;
using Simpra.Core.Repository;

namespace Simpra.Repository.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {

        }
    }
}
