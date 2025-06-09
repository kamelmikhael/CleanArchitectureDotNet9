using SharedKernal.Primitives;
using System.Linq.Expressions;

namespace Persistence.Specifications;

public abstract class Specification<TEntity> : Specification<TEntity, Guid>
    where TEntity : Entity
{
    protected Specification(Expression<Func<TEntity, bool>>? criteria)
        : base(criteria)
    { }

}

public abstract class Specification<TEntity, TKey>
    where TEntity : Entity<TKey>
{
    protected Specification(Expression<Func<TEntity, bool>>? criteria)
        => Criteria = criteria;

    public bool IsSplitQuery { get; protected set; }

    public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; private set; } = [];

    public Expression<Func<TEntity, object>>? OrderByExpressions { get; private set; }

    public Expression<Func<TEntity, object>>? OrderByDescendingExpressions { get; private set; }

    protected void Include(Expression<Func<TEntity, object>> expression)
        => IncludeExpressions.Add(expression);

    protected void OrderBy(Expression<Func<TEntity, object>> expression)
        => OrderByExpressions = expression;

    protected void OrderByDescending(Expression<Func<TEntity, object>> expression)
        => OrderByDescendingExpressions = expression;
}
