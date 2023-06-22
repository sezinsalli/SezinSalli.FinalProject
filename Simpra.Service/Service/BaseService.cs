using Microsoft.EntityFrameworkCore;
using Serilog;
using Simpra.Core.Entity;
using Simpra.Core.Repository;
using Simpra.Core.Service;
using Simpra.Core.UnitofWork;
using Simpra.Service.Exceptions;
using System.Linq.Expressions;


namespace Simpra.Service.Service
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public BaseService(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // Genel Yaklaşım: Fırlattığım exception'ı error için hazırladığım "UseCustomExceptionHandler "middleware de yakalayıp "Custom Response" dönüyorum.

        public virtual async Task<T> AddAsync(T entity, string changedBy = "admin")
        {
            try
            {
                entity.CreatedBy = changedBy;
                await _repository.AddAsync(entity);
                await _unitOfWork.CompleteAsync();
                return entity;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AddAsync Exception");
                throw new Exception($"{entity.GetType().Name} cannot create. Error message:{ex.Message}");
            }
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, string changedBy = "admin")
        {
            try
            {
                foreach (var entity in entities)
                {
                    entity.CreatedBy = changedBy;
                }
                await _repository.AddRangeAsync(entities);
                await _unitOfWork.CompleteAsync();
                return entities;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AddRangeAsync Exception");
                throw new Exception($"{entities.GetType().Name} cannot create. Error message:{ex.Message}");
            }
        }

        // Not Found durumunda hata fırlatmıyorum cevabı almam gerekiyor başka servislerde kullanırken.
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await _repository.AnyAsync(expression);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AnyAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAll().ToListAsync();
                return entities;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetAllAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }

        // Hata durumunda custom olarak oluşturduğum hata tipimi dönmeliyim bu sayede middleware de status codeları doğru ayarlayabilirim.
        public virtual async Task<T> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    throw new NotFoundException($"{typeof(T).Name} ({id}) not found!");

                return entity;
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    Log.Warning(ex, "GetByIdAsync Exception - Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                }
                Log.Error(ex, "GetByIdAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }

        public virtual async Task RemoveAsync(T entity)
        {
            try
            {
                _repository.Remove(entity);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "RemoveAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                _repository.RemoveRange(entities);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "RemoveRangeAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }

        public virtual async Task UpdateAsync(T entity, string changedBy = "admin")
        {
            try
            {
                var response = await _repository.AnyAsync(x => x.Id == entity.Id);
                if (!response)
                    throw new NotFoundException($"{typeof(T).Name} ({entity.Id}) not found!");

                entity.UpdatedBy = changedBy;
                _repository.Update(entity);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    Log.Warning(ex, "UpdateAsync Exception - Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                }
                Log.Error(ex, "UpdateAsync Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }

        public virtual IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            try
            {
                return _repository.Where(expression);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Where Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }

        public virtual IEnumerable<T> WhereWithInclude(Expression<Func<T, bool>> expression, params string[] includes)
        {
            try
            {
                return _repository.WhereWithInclude(expression, includes);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "WhereWithInclude Exception");
                throw new Exception($"Something went wrong. Error message:{ex.Message}");
            }
        }
    }
}
