using Carter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.MiddleWares;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<GlobalExceptionHandlingMiddleWare>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddCarter();

        return services;
    }
}
