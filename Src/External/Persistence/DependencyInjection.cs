using Domain.Customers;
using Domain.Orders;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using Persistence.Repositories.Customers;
using Persistence.Repositories.Orders;
using Scrutor;
using SharedKernal.Abstraction.Data;
using SharedKernal.Abstractions.Data;
using SharedKernal.Guards;

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

        Ensure.NotEmpty(connectionString);

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
            .AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            //.AddScoped<IOrderRepository, OrderRepository>()
            //.AddScoped<ICustomerRepository, CustomerRepository>()
            //.AddScoped<IUnitOfWork, UnitOfWork>();

        services
            .AddScoped<IUserRepository, UserRepository>()
            .Decorate<IUserRepository, CachedUserRepository>();

        services.Scan(selector =>
            selector
                .FromAssemblies(AssemblyReference.Assembly)
                .AddClasses(false)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime()
        );

        return services;
    }
}
