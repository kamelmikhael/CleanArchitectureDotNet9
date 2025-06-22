using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernal.Abstractions;
using Caching;
using Persistence;
using Application;
using Security;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddApplication()
            .AddPersistence(configuration)
            .AddSecurity()
            .AddServices()
            .AddCaching(configuration);
            //.AddBackgroundJobs();
            //.AddSagaRebus(configuration);
            //.AddMessageBroker(configuration)
            //.AddBackgroundJobs();
    }

    private static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddScoped<IEventPublisher, EventPublisher>();

        return services;
    }
}
