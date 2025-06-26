using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;
using Persistence.Specifications;
using SharedKernal.Abstraction.Data;
using SharedKernal.Primitives;

namespace Persistence.Repositories;

public class Repository<TEntity> : Repository<TEntity, Guid>, IRepository<TEntity>
    where TEntity : Entity
{
    public Repository(ApplicationDbContext context)
        : base(context)
    { }
}

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
{
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> FindAsync(
        TKey id, 
        CancellationToken cancellationToken = default) 
        => await _dbSet.FindAsync([id], cancellationToken: cancellationToken);

    public virtual async Task<TEntity?> FindAsync(
        object?[]? keyValues,
        CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync(keyValues, cancellationToken: cancellationToken);

    public async Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(cancellationToken);

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

    public virtual async Task<IEnumerable<TEntity>> ToListAsync(
        CancellationToken cancellationToken = default)
        => await _dbSet.ToListAsync(cancellationToken);

    public virtual async Task<IEnumerable<TEntity>> ToListAsync(
        string? sortOrder = null,
        Expression<Func<TEntity, object>>? keySelector = null,
        CancellationToken cancellationToken = default,
        params List<(bool condition, Expression<Func<TEntity, bool>> predicate)> predicates)
    {
        IQueryable<TEntity> query = _dbSet.AsQueryable();

        predicates.ForEach((item) => query = query.WhereIf(item.condition, item.predicate));

        if (keySelector is not null && sortOrder is not null)
        {
            query = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<TResult>> ToListAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        string? sortOrder = null,
        Expression<Func<TEntity, object>>? keySelector = null,
        CancellationToken cancellationToken = default,
        params List<(bool condition, Expression<Func<TEntity, bool>> predicate)> predicates)
    {
        IQueryable<TEntity> query = _dbSet.AsQueryable();

        predicates.ForEach((item) => query = query.WhereIf(item.condition, item.predicate));

        if (keySelector is not null && sortOrder is not null)
        {
            query = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }

        return await query.Select(selector).ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<TEntity>, int)> ToPagedListAsync(
        string? sortOrder = null,
        Expression<Func<TEntity, object>>? keySelector = null,
        int pageIndex = 0,
        int pageSize = 10,
        CancellationToken cancellationToken = default,
        params List<(bool condition, Expression<Func<TEntity, bool>> predicate)> predicates)
    {
        IQueryable<TEntity> query = _dbSet
            .AsNoTracking()
            .AsQueryable();

        predicates.ForEach((item) => query = query.WhereIf(item.condition, item.predicate));

        if (keySelector is not null && sortOrder is not null)
        {
            query = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }

        return (
            await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken),
            await query.CountAsync(cancellationToken)
        );
    }

    public async Task<(IEnumerable<TResult>, int)> ToPagedListAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        string? sortOrder = null,
        Expression<Func<TEntity, object>>? keySelector = null,
        int pageIndex = 0,
        int pageSize = 10,
        CancellationToken cancellationToken = default,
        params List<(bool condition, Expression<Func<TEntity, bool>> predicate)> predicates)
    {
        IQueryable<TEntity> query = _dbSet
            .AsNoTracking()
            .AsQueryable();

        predicates.ForEach((item) => query = query.WhereIf(item.condition, item.predicate));

        if (keySelector is not null && sortOrder is not null)
        {
            query = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
        }

        return (
            await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(selector)
                .ToListAsync(cancellationToken),
            await query.CountAsync(cancellationToken)
        );
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default) 
        => _dbSet.CountAsync(cancellationToken);

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => _dbSet.CountAsync(predicate, cancellationToken);

    public virtual void Add(TEntity entity)
        => _dbSet.Add(entity);

    public void AddRange(IEnumerable<TEntity> entities) 
        => _dbSet.AddRange(entities);

    public virtual void Update(TEntity entity)
        => _dbSet.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities) 
        => _dbSet.UpdateRange(entities);

    public virtual void Delete(TEntity entity)
        => _dbSet.Remove(entity);

    public void DeleteRange(IEnumerable<TEntity> entities) 
        => _dbSet.RemoveRange(entities);

    public virtual Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default)
        => _dbSet.AnyAsync(predicate, cancellationToken);

    protected virtual IQueryable<TEntity> ApplySpecification(
        Specification<TEntity, TKey> specification)
        => SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), specification);
}
