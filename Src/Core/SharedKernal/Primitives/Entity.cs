using SharedKernal.Abstraction;

namespace SharedKernal.Primitives;

public abstract class Entity : Entity<Guid>
{
    protected Entity() : base()
    { }

    protected Entity(Guid id) : base(id)
    { }
}

public abstract class Entity<TPrimaryKey> : IEquatable<Entity<TPrimaryKey>>, IEntity
{
    public virtual TPrimaryKey Id { get; private init; }

    private readonly List<IDomainEvent> _domainEvents = [];

    public List<IDomainEvent> DomainEvents => [.. _domainEvents];

    protected Entity()
    { }

    protected Entity(TPrimaryKey id)
    {
        Id = id;
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public bool Equals(Entity<TPrimaryKey>? other)
    {
        if (other is null || other.GetType() != GetType())
        {
            return false;
        }

        return EqualityComparer<TPrimaryKey>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Entity<TPrimaryKey>);
    }

    public override int GetHashCode()
    {
        return Id!.GetHashCode() * 41;
    }

    public static bool operator ==(Entity<TPrimaryKey> first, Entity<TPrimaryKey> second)
        => first.Equals(second);

    public static bool operator !=(Entity<TPrimaryKey> first, Entity<TPrimaryKey> second)
        => !first.Equals(second);
}
