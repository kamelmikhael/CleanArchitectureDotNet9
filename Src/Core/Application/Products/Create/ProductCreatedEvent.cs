using SharedKernal.Abstraction;

namespace Application.Products.Create;

public sealed record ProductCreatedEvent(Guid ProductId) : IDomainEvent;
