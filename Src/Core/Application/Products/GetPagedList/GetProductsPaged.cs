using System.Linq.Expressions;
using Application.Abstractions.Messaging;
using Domain.Products;
using SharedKernal.Abstraction.Data;
using SharedKernal.Primitives;

namespace Application.Products.GetPagedList;

public sealed class GetProductsPaged
{
    public sealed record Query(ProductsPagedRequest Input) : IPagedQuery<ProductsPagedResponse>;

    public sealed class Handler(
        IRepository<Product, ProductId> repository) : IPagedQueryHandler<Query, ProductsPagedResponse>
    {
        public async Task<PagedResult<ProductsPagedResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            (IEnumerable<ProductsPagedResponse>? Products, int totalCount) = await repository.ToPagedListAsync(
                p => new ProductsPagedResponse(p.Id.Value
                                               , p.Name
                                               , p.Sku.Value
                                               , p.Price.Amount
                                               , p.Price.Currency),
                query.Input.SortOrder,
                GetSortProperty(query.Input.SortColumn),
                query.Input.PageIndex,
                query.Input.PageSize,
                cancellationToken,
                (
                    !string.IsNullOrWhiteSpace(query.Input.SearchTerm),
                    p => p.Name.Contains(query.Input.SearchTerm!)
                        || ((string)p.Sku).Contains(query.Input.SearchTerm!)
                ));

            return PagedResult<ProductsPagedResponse>.Create(
                [.. Products],
                totalCount,
                query.Input.PageIndex,
                query.Input.PageSize);
        }

        private static Expression<Func<Product, object>> GetSortProperty(string? sortColumn)
            => sortColumn?.ToLower() switch
            {
                "name" => product => product.Name,
                "sku" => product => product.Sku,
                "amount" => product => product.Price.Amount,
                "currency" => product => product.Price.Currency,
                _ => product => product.Id,
            };
    }
}
