using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Presentation.MiddleWares;

public class GlobalExceptionHandlingMiddleWare : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleWare> _logger;

    public GlobalExceptionHandlingMiddleWare(ILogger<GlobalExceptionHandlingMiddleWare> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            ProblemDetails problemDetails = new()
            {
                Status = context.Response.StatusCode,
                Type = "Server error",
                Title = "Server error",
                Detail = "An unexpected internal server error has occurred.",
            };

            string json = JsonSerializer.Serialize(problemDetails);

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(json);
        }
    }
}
