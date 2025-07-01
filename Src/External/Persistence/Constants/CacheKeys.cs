using Domain.Users;

namespace Persistence.Constants;

public static class CacheKeys
{
    public static Func<Guid, string> UserById = userId => $"User-Id-{userId}";
    public static Func<UserName, string> UserByUsername = userName => $"User-Email-{userName.Value}";
}
