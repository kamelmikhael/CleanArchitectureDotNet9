using SharedKernal.Abstraction;

namespace Application.Products.Delete;

public sealed record ProductDeletedEvent(Guid ProductId) : IDomainEvent;
