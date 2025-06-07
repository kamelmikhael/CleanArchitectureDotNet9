using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernal.Abstraction.Caching;

namespace Caching;

public static class DependencyInjection
{
    public static IServiceCollection AddCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddMemoryCache();
        services.AddDistributedMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();

        //services.AddStackExchangeRedisCache(options =>
        //    options.Configuration = configuration.GetConnectionString("Redis"));

        return services;
    }
}
