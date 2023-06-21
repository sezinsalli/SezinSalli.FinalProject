using Simpra.Core.Entity;
using System.Linq.Expressions;

namespace Simpra.Core.Service
{
    public interface IBaseService<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task<T> AddAsync(T entity, string changedBy);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, string changedBy);
        Task UpdateAsync(T entity, string changedBy);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
        IEnumerable<T> WhereWithInclude(Expression<Func<T, bool>> expression, params string[] includes);
    }
}
