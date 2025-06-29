using System.Threading.RateLimiting;
using Carter;
using Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Presentation;
using Persistence.Extensions;
using Presentation.MiddleWares;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
    app.ApplyMigrations();
}

// used to log HTTP Api request coming to Our ASP API
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleWare>();

app.UseRateLimiter();

app.MapCarter();

app.Run();

public partial class Program { }
