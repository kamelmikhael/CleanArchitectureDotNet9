using Domain.Customers;
using Domain.Orders;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Interceptors;
using Persistence.Repositories;
using Persistence.Repositories.Customers;
using Persistence.Repositories.Orders;
using Scrutor;
using SharedKernal.Abstraction.Data;
using SharedKernal.Abstractions.Data;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddDatabase(configuration);
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
            .AddScoped(typeof(IRepository<,>), typeof(Repository<,>))
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .Decorate<IUserRepository, CachedUserRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
