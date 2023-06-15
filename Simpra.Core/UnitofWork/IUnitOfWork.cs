namespace Simpra.Core.UnitofWork
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
        Task CompleteWithTransactionAsync();
    }
}
