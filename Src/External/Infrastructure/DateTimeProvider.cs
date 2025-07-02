﻿using SharedKernal.Abstraction;

namespace Infrastructure;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
