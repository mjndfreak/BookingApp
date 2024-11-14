using System.Linq.Expressions;
using BookingApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly BookingAppDbContext _db;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(BookingAppDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<TEntity>();
    }
    
    public void Add(TEntity entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        _dbSet.Add(entity);
        //_db.SaveChanges();
    }

    public void Delete(TEntity entity, bool isSoftDelete = true)
    {
        if (isSoftDelete)
        {
            entity.ModifiedAt = DateTime.UtcNow;
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }
        _dbSet.Remove(entity);
    }
    
    

    public void Delete(int id)
    {
        var entity = _dbSet.Find(id);
        Delete(entity);
    }

    public void Update(TEntity entity)
    {
        entity.ModifiedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
        //_db.SaveChanges();
    }

    public TEntity GetById(int id)
    {
        return  _dbSet.Find(id);  }

    public TEntity Get(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.FirstOrDefault(predicate);
    }

    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
    {
        return predicate == null ? _dbSet : _dbSet.Where(predicate);
    }
}