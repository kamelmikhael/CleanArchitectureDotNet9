using SharedKernal.Primitives;
using System.Linq.Expressions;

namespace SharedKernal.Abstraction.Data;

public interface IRepository<TEntity> : IRepository<TEntity, Guid>
    where TEntity : Entity
{ }

public interface IRepository<TEntity, TKey> 
    where TEntity : Entity<TKey>
{
    Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken = default);

    Task<TEntity?> FindAsync(object?[]? keyValues, CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

    Task<TResult?> FirstOrDefaultAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default);

    Task<TResult?> FirstOrDefaultAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> ToListAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> ToListAsync(
        string? sortOrder = null,
        Expression<Func<TEntity, object>>? keySelector = null,
        CancellationToken cancellationToken = default,
        params List<(bool condition, Expression<Func<TEntity, bool>> predicate)> predicates);

    Task<IEnumerable<TResult>> ToListAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        string? sortOrder = null,
        Expression<Func<TEntity, object>>? keySelector = null,
        CancellationToken cancellationToken = default,
        params List<(bool condition, Expression<Func<TEntity, bool>> predicate)> predicates);

    Task<(IEnumerable<TEntity>, int)> ToPagedListAsync(
        string? sortOrder = null,
        Expression<Func<TEntity, object>>? keySelector = null,
        int pageIndex = 0,
        int pageSize = 10,
        CancellationToken cancellationToken = default,
        params List<(bool condition, Expression<Func<TEntity, bool>> predicate)> predicates);

    Task<(IEnumerable<TResult>, int)> ToPagedListAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        string? sortOrder = null,
        Expression<Func<TEntity, object>>? keySelector = null,
        int pageIndex = 0,
        int pageSize = 10,
        CancellationToken cancellationToken = default,
        params List<(bool condition, Expression<Func<TEntity, bool>> predicate)> predicates);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);

    void DeleteRange(IEnumerable<TEntity> entities);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}
