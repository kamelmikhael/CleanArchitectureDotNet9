namespace SharedKernal.Abstraction;

public interface IAuditableEntity
{
    DateTime CreatedOnUtc { get; set; }

    DateTime? UpdatedOnUtc { get; set; }
}
