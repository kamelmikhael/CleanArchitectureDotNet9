using Microsoft.EntityFrameworkCore;
using SharedKernal.Primitives;

namespace Persistence.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity, TKey>(
        IQueryable<TEntity> inputQuery,
        Specification<TEntity, TKey> specification) where TEntity : Entity<TKey>
    {
        IQueryable<TEntity> query = inputQuery;

        if (specification.IsSplitQuery)
        {
            query = query.AsSplitQuery();
        }

        query = specification.IncludeExpressions
            .Aggregate(query, (current, includeExpression) => current.Include(includeExpression));

        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        if (specification.OrderByExpressions is not null)
        {
            query = query.OrderBy(specification.OrderByExpressions);
        }
        else if (specification.OrderByDescendingExpressions is not null)
        {
            query = query.OrderByDescending(specification.OrderByDescendingExpressions);
        }

        return query;
    }
}
