using Application;
using Application.Abstractions.Messaging;
using Application.Users.Login;
using Application.Users.Register;
using Carter;
using Infrastructure;
using Persistence;
using Presentation;
using Presentation.MiddleWares;
using SharedKernal.Primitives;
using MessageBroker;
using Caching;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddPersistence(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddPresentation(builder.Configuration)
    .AddCaching(builder.Configuration);
//.AddMessageBroker(builder.Configuration);

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
}

// used to log HTTP Api request coming to Our ASP API
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleWare>();

app.MapCarter();

app.Run();


