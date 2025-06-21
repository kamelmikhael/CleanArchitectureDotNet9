using SharedKernal.Abstraction;

namespace Application.Products.Update;

internal sealed record ProductUpdatedEvent(Guid Id) : IDomainEvent;
