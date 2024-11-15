using BookingApp.Data.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookingApp.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly BookingAppDbContext _db;
    private IDbContextTransaction? _transaction;
    
    public UnitOfWork(BookingAppDbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }
    
    public void Dispose()
    {
        _db.Dispose();
        // This is where I give permission to the garbage collector to clean up the object.
        //GC.Collect();
        //GC.WaitForPendingFinalizers();
        //These will force the garbage collector to clean up the object.
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _db.Database.BeginTransactionAsync();
    }

    public async Task CommitTransaction()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackTransaction()
    {
        await _transaction.RollbackAsync();
    }
}