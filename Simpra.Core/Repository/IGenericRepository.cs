using Simpra.Core.Entity;
using System.Linq.Expressions;

namespace Simpra.Core.Repository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAll();
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<List<T>> GetAllWithIncludeAsync(params string[] includes);
        Task<T> GetByIdWithIncludeAsync(int id, params string[] includes);
        IEnumerable<T> WhereWithInclude(Expression<Func<T, bool>> expression, params string[] includes);

    }
}
