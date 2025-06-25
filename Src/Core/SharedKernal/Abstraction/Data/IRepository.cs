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

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> ToListAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> ToListAsync(
        List<(bool condition, Expression<Func<TEntity, bool>> predicate)> predicates,
        CancellationToken cancellationToken = default);

    Task<(IEnumerable<TEntity>, int)> ToPagedListAsync(
        int pageIndex = 0,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    Task<(IEnumerable<TEntity>, int)> ToPagedListAsync(Expression<Func<TEntity, bool>> predicate,
        int pageIndex = 0,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    Task<(IEnumerable<TEntity>, int)> ToPagedListAsync(
        List<(bool condition, Expression<Func<TEntity, bool>> predicate)> predicates,
        int pageIndex = 0,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

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
