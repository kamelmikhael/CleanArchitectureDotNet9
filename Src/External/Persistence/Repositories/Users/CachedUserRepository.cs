using Domain.Users;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Persistence.Resolvers;

namespace Persistence.Repositories;

public sealed class CachedUserRepository(
    ApplicationDbContext context,
    IUserRepository decorated,
    IMemoryCache memoryCache,
    IDistributedCache distributedCache) : Repository<User>(context), IUserRepository
{
    public override Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"User-{id}";

        return memoryCache.GetOrCreateAsync(cacheKey, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
            return decorated.GetByIdAsync(id, cancellationToken);
        });
    }

    public async Task<User?> GetByIdAsync_UsingRedis(Guid id, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"User-{id}";

        string? cachedUser = await distributedCache.GetStringAsync(cacheKey, cancellationToken);

        if (string.IsNullOrEmpty(cachedUser))
        {
            var user = await decorated.GetByIdAsync(id, cancellationToken);

            if (user is not null)
            {
                await distributedCache.SetStringAsync(
                    cacheKey,
                    JsonConvert.SerializeObject(user),
                    cancellationToken);
            }

            return user;
        }

        return JsonConvert.DeserializeObject<User>(cachedUser,
            new JsonSerializerSettings { 
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateResolver()
            });
    }

    public Task<User?> GetByUsernameAsync(UserName username, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"User-{username}";

        return memoryCache.GetOrCreateAsync(cacheKey, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
            return decorated.GetByUsernameAsync(username, cancellationToken);
        });
    }

    public Task<bool> IsEmailExistsAsync(Email email, CancellationToken cancellationToken = default)
    {
       return decorated.IsEmailExistsAsync(email, cancellationToken);
    }
}
