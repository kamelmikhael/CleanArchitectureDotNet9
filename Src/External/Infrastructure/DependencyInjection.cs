﻿using Application;
using Application.Orders.Services;
using Caching;
using Infrastructure.Orders.Services;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.AddScoped<IGetOrderByIdService, GetOrderByIdService>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
