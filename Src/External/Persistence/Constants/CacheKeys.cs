using Domain.Users;

namespace Persistence.Constants;

public static class CacheKeys
{
    public static readonly Func<Guid, string> UserById = userId => $"User-Id-{userId}";
    public static readonly Func<UserName, string> UserByUsername = userName => $"User-Email-{userName.Value}";
}
