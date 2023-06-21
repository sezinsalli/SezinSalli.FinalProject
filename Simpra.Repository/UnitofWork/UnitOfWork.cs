using Serilog;
using Simpra.Core.UnitofWork;

namespace Simpra.Repository.UnitofWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed;
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Complete()
        {
            _context.SaveChanges();
        }

        public void CompleteWithTransaction()
        {
            using (var dbDcontextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    dbDcontextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "CompleteWithTransaction Exception");                    
                    dbDcontextTransaction.Rollback();
                    throw new Exception($"Something went wrong! Error message:{ex.Message}");
                }
            }
        }

        public async Task CompleteWithTransactionAsync()
        {
            await using (var dbDcontextTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    await dbDcontextTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "CompleteWithTransactionAsync Exception");
                    await dbDcontextTransaction.RollbackAsync();
                    throw new Exception($"Something went wrong! Error message:{ex.Message}");
                }
            }
        }

        private void Clean(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            disposed = true;
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            Clean(true);
        }
    }
}
