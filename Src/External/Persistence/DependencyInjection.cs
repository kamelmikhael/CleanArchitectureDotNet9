using SharedKernal.Abstractions.Data;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Interceptors;
using Persistence.Repositories;
using Scrutor;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddDatabase(configuration)
            .AddRedisCaching(configuration);
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        //services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        string? connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ApplicationDbContext>(
            (sp, optionsBuilder) =>
            {
                //var convertDomainEventsToOutboxMessagesInterceptor = 
                //    sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();

                //var updateAuditableEntitiesInterceptor = 
                //    sp.GetRequiredService<UpdateAuditableEntitiesInterceptor>();

                optionsBuilder
                    .UseSqlServer(connectionString);
                    //.AddInterceptors(convertDomainEventsToOutboxMessagesInterceptor
                    //    , updateAuditableEntitiesInterceptor);
            });

        services
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped<IUserRepository, UserRepository>()
            .Decorate<IUserRepository, CachedUserRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddRedisCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMemoryCache();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        return services;
    }
}
