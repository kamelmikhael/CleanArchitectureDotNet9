using System.Linq.Expressions;

namespace Persistence.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TSource> WhereIf<TSource>(
        this IQueryable<TSource> query,
        bool condition,
        Expression<Func<TSource, bool>> predicate)
    {
        if (condition)
        {
            query = query.Where(predicate);
        }

        return query;
    }
}
