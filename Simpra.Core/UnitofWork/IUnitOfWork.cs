namespace Simpra.Core.UnitofWork
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
        void Complete();
        Task CompleteWithTransactionAsync();
        void CompleteWithTransaction();
    }
}
