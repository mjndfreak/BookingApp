using BookingApp.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookingApp.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    // Private field to hold the DbContext instance
    private readonly BookingAppDbContext _db;
    // Private field to hold the database transaction instance
    private IDbContextTransaction _transaction;
    
    // Constructor to initialize the UnitOfWork with a DbContext instance
    public UnitOfWork(BookingAppDbContext db)
    {
        // Check if the provided DbContext is null and throw an exception if so
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }
    
    // Dispose method to release the resources used by the DbContext
    public void Dispose()
    {
        _db.Dispose(); // Properly dispose the DbContext
        // This is where I give permission to the garbage collector to clean up the object.
        // GC.Collect(); // Uncomment to force garbage collection (not recommended in general)
        // GC.WaitForPendingFinalizers(); // Waits for finalizers to complete
        // These will force the garbage collector to clean up the object.
    }

    // Method to save changes asynchronously to the database
    public async Task<int> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync(); // Save changes and return the number of state entries written to the database
    }

    // Method to begin a new transaction asynchronously
    public async Task BeginTransactionAsync()
    {
        _transaction = await _db.Database.BeginTransactionAsync(); // Start a new transaction
    }

    // Method to commit the current transaction asynchronously
    public async Task CommitTransaction()
    {
        await _transaction.CommitAsync(); // Commit the transaction to save changes permanently
    }

    // Method to roll back the current transaction asynchronously in case of an error
    public async Task RollbackTransaction()
    {
        await _transaction.RollbackAsync(); // Roll back the transaction to revert changes
    }
}