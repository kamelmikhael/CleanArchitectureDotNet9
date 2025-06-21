using SharedKernal.Abstraction;

namespace Domain.Products;

public sealed record ProductUpdatedDomainEvent(Guid Id) : IDomainEvent;
