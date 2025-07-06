using System.Threading.RateLimiting;
using Api.Middelware;
using Carter;
using HealthChecks.UI.Client;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Persistence.Extensions;
using Presentation;
using Presentation.MiddleWares;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddPresentation(builder.Configuration);

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 3;
        options.QueueLimit = 3;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    //rateLimiterOptions.AddSlidingWindowLimiter("sliding", options =>
    //{
    //    options.Window = TimeSpan.FromSeconds(15);
    //    options.SegmentsPerWindow = 3;
    //    options.PermitLimit = 15;
    //    options.QueueLimit = 3;
    //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    //});

    //rateLimiterOptions.AddTokenBucketLimiter("token", options =>
    //{
    //    options.TokenLimit = 100;
    //    options.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
    //    options.TokensPerPeriod = 10;
    //});

    //rateLimiterOptions.AddConcurrencyLimiter("concurrency", options =>
    //{
    //    options.PermitLimit = 5;
    //});
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

// used to log HTTP Api request coming to Our ASP API
app.UseSerilogRequestLogging();

app.UseGlobalExceptionHandler();

app.UseMiddleware<RequestLogContextMiddelware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapCarter();

await app.RunAsync();

public partial class Program { }
