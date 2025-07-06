using System.Threading.RateLimiting;
using Api.Middelware;
using Api.OpenApi;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
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

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

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

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();

RouteGroupBuilder versionGroup = app
    .MapGroup("api/v{apiVersion:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

versionGroup.MapCarter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

        foreach (ApiVersionDescription description in descriptions)
        {
            string url = $"/swagger/{description.GroupName}/swagger.json";
            string name = description.GroupName.ToUpperInvariant();

            options.SwaggerEndpoint(url, name);
        }
    });

    //app.MapOpenApi();

    app.ApplyMigrations();
}

await app.RunAsync();

public partial class Program { }
