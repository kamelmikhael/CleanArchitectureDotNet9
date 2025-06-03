namespace SharedKernal.Abstraction;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
