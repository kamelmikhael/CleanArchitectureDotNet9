using SharedKernal.Abstraction;

namespace Domain.Products;

public sealed record ProductCreatedDomainEvent(Guid ProductId) : IDomainEvent;
