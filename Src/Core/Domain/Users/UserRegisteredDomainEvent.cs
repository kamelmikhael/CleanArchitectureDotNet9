using SharedKernal.Abstraction;

namespace Domain.Users;

public sealed record UserRegisteredDomainEvent(Guid UserId) : IDomainEvent;
