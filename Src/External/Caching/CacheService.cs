using Microsoft.Extensions.Caching.Distributed;
using SharedKernal.Abstraction.Caching;
using Newtonsoft.Json;
using Caching.Resolvers;
using System.Collections.Concurrent;

namespace Caching;

public class CacheService(
    IDistributedCache distributedCache) : ICacheService
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();

    private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
    {
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        ContractResolver = new PrivateResolver()
    };

    public async Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default) 
        where T : class
    {
        string? cachedValue = await distributedCache.GetStringAsync(cacheKey, cancellationToken);

        if (cachedValue is null)
        {
            return null;  
        }

        T? value = JsonConvert.DeserializeObject<T>(cachedValue, _jsonSerializerSettings);

        return value;
    }

    public async Task<T?> GetOrCreateAsync<T>(string cacheKey, Func<Task<T?>> factory, CancellationToken cancellationToken = default)
        where T : class
    {
        T? cachedValue = await GetAsync<T>(cacheKey, cancellationToken);

        if (cachedValue is not null)
        {
            return cachedValue;
        }

        cachedValue = await factory();

        if(cachedValue is not null)
        {
            await SetAsync(cacheKey, cachedValue, cancellationToken);
        }

        return cachedValue;
    }

    public async Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(cacheKey, cancellationToken);

        CacheKeys.TryRemove(cacheKey, out bool _);
    }

    public async Task RemoveByPrefixAsync(string cachePrefixKey, CancellationToken cancellationToken = default)
    {
        IEnumerable<Task> tasks = CacheKeys
            .Keys
            .Where(k => k.StartsWith(cachePrefixKey, StringComparison.OrdinalIgnoreCase))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(tasks);
    }

    public async Task SetAsync<T>(string cacheKey, T value, CancellationToken cancellationToken = default) where T : class
    {
        string cachedValue = JsonConvert.SerializeObject(value, _jsonSerializerSettings);

        await distributedCache.SetStringAsync(cacheKey, cachedValue, cancellationToken);

        CacheKeys.TryAdd(cacheKey, false);
    }
}
