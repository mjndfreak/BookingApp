namespace BookingApp.Data.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
    
    Task BeginTransactionAsync();
    
    Task CommitTransaction();
    
    Task RollbackTransaction();
}