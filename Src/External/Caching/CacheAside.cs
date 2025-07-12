using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Caching;

public static class CacheAside
{
    private static readonly DistributedCacheEntryOptions _defaultOptions = new() 
    { 
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
    };

    private static readonly SemaphoreSlim _semaphore = new(1,1);

    public static async Task<T?> GetOrCreate<T>(
        this IDistributedCache cache
        , string key
        , Func<CancellationToken, Task<T?>> factory
        , DistributedCacheEntryOptions? options = null
        , CancellationToken cancellationToken = default)
    {
        T? value;

        string? cachedValue = await cache.GetStringAsync(key, cancellationToken);

        if (!string.IsNullOrWhiteSpace(cachedValue))
        {
            value = JsonSerializer.Deserialize<T>(cachedValue);

            if (value is not null)
            {
                return value;
            }
        }

        bool hasLock = await _semaphore.WaitAsync(500, cancellationToken);

        if (hasLock)
        {
            return default;
        }

        try
        {
            cachedValue = await cache.GetStringAsync(key, cancellationToken);

            if (!string.IsNullOrWhiteSpace(cachedValue))
            {
                value = JsonSerializer.Deserialize<T>(cachedValue);

                if (value is not null)
                {
                    return value;
                }
            }

            value = await factory(cancellationToken);

            if (value is not null)
            {
                await cache.SetStringAsync(
                    key,
                    JsonSerializer.Serialize(value),
                    options ?? _defaultOptions,
                    cancellationToken);
            }
        }
        finally
        {
            _semaphore.Release();
        }

        return value;
    }
}
