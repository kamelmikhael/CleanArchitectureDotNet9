using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Authentication;

// Using for normal controllers
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
internal sealed class ApiKeyAttribute : Attribute, IAuthorizationFilter
{
    private const string API_KEY_HEADER_NAME = "X-Api-Key";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!IsApiKeyValid(context.HttpContext))
        {
            context.Result = new UnauthorizedResult();
        }
    }

    private bool IsApiKeyValid(HttpContext context)
    {
        string? apiKey = context.Request.Headers[API_KEY_HEADER_NAME];

        if (string.IsNullOrEmpty(apiKey))
        {
            return false;
        }

        string actualApiKey = context
            .RequestServices
            .GetRequiredService<IConfiguration>()
            .GetValue<string>("ApiKey")!;

        return actualApiKey.Equals(apiKey, StringComparison.OrdinalIgnoreCase);
    }
}
