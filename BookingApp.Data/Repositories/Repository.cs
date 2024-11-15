using System.Linq.Expressions;
using BookingApp.Data.Context;
using BookingApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    // Private field to hold the database context
    private readonly BookingAppDbContext _db;
    // Private field to hold the DbSet representing the entity set
    private readonly DbSet<TEntity> _dbSet;

    // Constructor to initialize the repository with a DbContext instance
    public Repository(BookingAppDbContext db)
    {
        _db = db; // Assign the passed DbContext to the private field
        _dbSet = _db.Set<TEntity>(); // Get the DbSet for the specified TEntity type
    }
    
    // Method to add a new entity to the DbSet
    public void Add(TEntity entity)
    {
        entity.CreatedAt = DateTime.UtcNow; // Set the CreatedAt property to the current UTC time
        _dbSet.Add(entity); // Add the entity to the DbSet
        //_db.SaveChanges(); // Uncomment if immediate save is desired (typically handled by UnitOfWork)
    }

    // Method to delete an entity, supporting soft deletion
    public void Delete(TEntity entity, bool isSoftDelete = true)
    {
        if (isSoftDelete) // Check if soft deletion is enabled
        {
            entity.ModifiedAt = DateTime.UtcNow; // Update the ModifiedAt property
            entity.IsDeleted = true; // Mark the entity as deleted
            _dbSet.Update(entity); // Update the entity state in the DbSet
        }
        _dbSet.Remove(entity); // Physically remove the entity from the DbSet
    }
    
    // Method to delete an entity by its ID
    public void Delete(int id)
    {
        var entity = _dbSet.Find(id); // Find the entity by ID
        Delete(entity); // Call the Delete method to remove it
    }

    // Method to update an existing entity
    public void Update(TEntity entity)
    {
        entity.ModifiedAt = DateTime.UtcNow; // Update the ModifiedAt property to the current UTC time
        _dbSet.Update(entity); // Update the entity state in the DbSet
        //_db.SaveChanges(); // Uncomment if immediate save is desired (typically handled by UnitOfWork)
    }

    // Method to get an entity by its ID
    public TEntity GetById(int id)
    {
        return _dbSet.Find(id); // Find and return the entity by its ID
    }

    // Method to get a single entity based on a predicate condition
    public TEntity Get(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.FirstOrDefault(predicate); // Return the first entity that matches the condition or null if not found
    }

    // Method to get all entities, optionally filtered by a predicate
    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
    {
        return predicate == null ? _dbSet : _dbSet.Where(predicate); // Return all entities or filtered ones based on the predicate
    }
}