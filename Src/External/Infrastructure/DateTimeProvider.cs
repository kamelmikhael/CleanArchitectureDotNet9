using SharedKernal.Abstraction;

namespace Infrastructure;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
