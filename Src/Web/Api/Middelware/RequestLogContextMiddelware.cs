using Serilog.Context;

namespace Api.Middelware;

public class RequestLogContextMiddelware
{
    private readonly RequestDelegate _next;

    public RequestLogContextMiddelware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context)
    {
        using (LogContext.PushProperty("CorrelattionId", context.TraceIdentifier))
        {
            return _next(context);
        }
    }
}
