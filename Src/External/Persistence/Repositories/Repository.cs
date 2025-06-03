using SharedKernal.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using SharedKernal.Primitives;
using System.Linq.Expressions;
using Persistence.Specifications;

namespace Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
{
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public virtual Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public virtual Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _dbSet.ToListAsync(cancellationToken);
    }

    public virtual void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _dbSet.AnyAsync(predicate, cancellationToken);
    }

    protected IQueryable<TEntity> ApplySpecification(Specification<TEntity> specification)
    {
        return SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), specification);
    }
}
