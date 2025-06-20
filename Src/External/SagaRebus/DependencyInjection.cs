using Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;
using SagaRebus.Orders;

namespace SagaRebus;

public static class DependencyInjection
{
    public static IServiceCollection AddSagaRebus(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        services.AddRebus(rebus => rebus
            .Routing(r => 
                r.TypeBased().MapAssemblyOf<ApplicationAssemblyReference>("eshop-queue"))
            .Transport(t => 
                t.UseRabbitMq(configuration.GetConnectionString("Reduis"), "eshop-queue"))
            .Sagas(s => 
                s.StoreInSqlServer(configuration.GetConnectionString("Database"),
                                    "Sagas",
                                    "Saga_Indexes")),
            onCreated: async bus =>
            {
                await bus.Subscribe<OrderConfirmationEmailSent>();
                await bus.Subscribe<OrderPaymentRequestSent>();
            });

        services.AutoRegisterHandlersFromAssemblyOf<ApplicationAssemblyReference>();

        return services;
    }
}
