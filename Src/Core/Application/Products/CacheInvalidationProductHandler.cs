using Application.Products.Create;
using Application.Products.Delete;
using Application.Products.Update;
using SharedKernal.Abstraction;
using SharedKernal.Abstraction.Caching;

namespace Application.Products;

internal sealed class CacheInvalidationProductHandler(
    ICacheService cacheService)
    : IDomainEventHandler<ProductCreatedEvent>
    , IDomainEventHandler<ProductUpdatedEvent>
    , IDomainEventHandler<ProductDeletedEvent>
{
    public async Task HandleAsync(ProductCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await HandleInternalAsync(cancellationToken);
    }

    public async Task HandleAsync(ProductUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await HandleInternalAsync(cancellationToken);
    }

    public async Task HandleAsync(ProductDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        await HandleInternalAsync(cancellationToken);
    }

    private async Task HandleInternalAsync(CancellationToken cancellationToken)
    {
        await cacheService.RemoveAsync("products", cancellationToken);
    }
}
