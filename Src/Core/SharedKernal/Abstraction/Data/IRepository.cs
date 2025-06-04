using SharedKernal.Primitives;
using System.Linq.Expressions;

namespace SharedKernal.Abstractions.Data;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> GetListAsync(CancellationToken cancellationToken = default);

    void Add(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}
