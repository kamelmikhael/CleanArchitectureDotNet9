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

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddPersistence(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddPresentation(builder.Configuration);
    //.AddMessageBroker(builder.Configuration);

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
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleWare>();

app.MapCarter();

app.Run();


