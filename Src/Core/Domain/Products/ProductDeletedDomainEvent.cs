using SharedKernal.Abstraction;

namespace Domain.Products;

public sealed record ProductDeletedDomainEvent(Guid ProductId) : IDomainEvent;
