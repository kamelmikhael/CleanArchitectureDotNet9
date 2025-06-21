using Domain.Products;
using SharedKernal.Abstraction;
using SharedKernal.Abstraction.Caching;

namespace Application.Products;

internal sealed class CacheInvalidationProductHandler(
    ICacheService cacheService)
    : IDomainEventHandler<ProductCreatedDomainEvent>
    , IDomainEventHandler<ProductUpdatedDomainEvent>
    , IDomainEventHandler<ProductDeletedDomainEvent>
{
    public async Task HandleAsync(ProductCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await HandleInternalAsync(cancellationToken);
    }

    public async Task HandleAsync(ProductUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await HandleInternalAsync(cancellationToken);
    }

    public async Task HandleAsync(ProductDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await HandleInternalAsync(cancellationToken);
    }

    private async Task HandleInternalAsync(CancellationToken cancellationToken)
    {
        await cacheService.RemoveAsync("products", cancellationToken);
    }
}
