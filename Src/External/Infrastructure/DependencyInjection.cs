using Application;
using Application.Orders.Services;
using Caching;
using Infrastructure.Health;
using Infrastructure.Orders.Services;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Persistence;
using Security;
using SharedKernal.Abstraction;
using SharedKernal.Abstractions;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddApplication()
            .AddPersistence(configuration)
            .AddSecurity()
            .AddServices()
            .AddCaching(configuration);
        //.AddBackgroundJobs();
        //.AddSagaRebus(configuration);
        //.AddMessageBroker(configuration)
        //.AddBackgroundJobs();

        services
            .AddHealthChecks()
            //.AddCheck<DatabaseHealthCheck>("custom-sql", HealthStatus.Unhealthy)
            .AddDbContextCheck<ApplicationDbContext>();

        return services;
    }

    private static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.AddScoped<IGetOrderByIdService, GetOrderByIdService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
