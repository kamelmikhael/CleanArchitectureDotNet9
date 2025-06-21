using Application.Abstractions.Messaging;
using Domain.Products;
using SharedKernal.Abstraction.Caching;
using SharedKernal.Abstractions.Data;
using SharedKernal.Primitives;

namespace Application.Products.GetList;

public sealed class GetProductList
{
    public sealed record Query : IQuery<List<GetProductListResponse>?>;

    internal sealed class Handler(
        IRepository<Product, ProductId> repository
        , ICacheService cacheService) : IQueryHandler<Query, List<GetProductListResponse>?>
    {
        public async Task<Result<List<GetProductListResponse>?>> Handle(Query query, CancellationToken cancellationToken)
        {
            return await cacheService.GetOrCreateAsync("products", async () =>
                    {
                        IEnumerable<Product> products = await repository.ToListAsync(cancellationToken);

                        return products
                            .Select(p => new GetProductListResponse(p.Id.Value, p.Name))
                            .ToList();
                    }, cancellationToken);
        }
    }
}
