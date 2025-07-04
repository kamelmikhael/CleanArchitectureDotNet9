using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Presentation.MiddleWares;

public class GlobalExceptionHandlingMiddleWare(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlingMiddleWare> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex
                , "Exception occurred: {Message} at {Time} UTC"
                , ex.Message
                , DateTime.UtcNow);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            ProblemDetails problemDetails = new()
            {
                Status = context.Response.StatusCode,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Title = "Server error",
                Detail = "An unexpected internal server error has occurred.",
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
