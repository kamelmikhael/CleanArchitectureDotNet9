namespace SharedKernal.Abstraction;

public interface IEntity
{
    List<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}
