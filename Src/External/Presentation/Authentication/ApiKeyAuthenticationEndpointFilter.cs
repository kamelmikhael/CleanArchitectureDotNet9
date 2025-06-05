using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Presentation.Authentication;

internal class ApiKeyAuthenticationEndpointFilter(
    IConfiguration configuration) : IEndpointFilter
{
    private const string API_KEY_HEADER_NAME = "X-Api-Key";

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context, 
        EndpointFilterDelegate next)
    {
        // You can pass Api key in Query String, Cookie, Header
        string? apiKey = context.HttpContext.Request.Headers[API_KEY_HEADER_NAME];

        if(!IsApiKeyValid(apiKey))
        {
            return Results.Unauthorized();
        }

        return await next(context);
    }

    private bool IsApiKeyValid(string? apiKey)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            return false;
        }

        string actualApiKey = configuration.GetValue<string>("ApiKey")!;

        return actualApiKey.Equals(apiKey, StringComparison.OrdinalIgnoreCase);
    }
}
