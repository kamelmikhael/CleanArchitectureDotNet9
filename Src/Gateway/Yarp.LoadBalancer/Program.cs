using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services
    .AddAuthentication(BearerTokenDefaults.AuthenticationScheme)
    .AddBearerToken();

builder.Services.AddHealthChecks();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapReverseProxy();

app.MapGet("login", () =>
    Results.SignIn(
        new ClaimsPrincipal(
            new ClaimsIdentity(
                [
                    new Claim("sub", Guid.NewGuid().ToString()),

                ],
                BearerTokenDefaults.AuthenticationScheme)),
        authenticationScheme: BearerTokenDefaults.AuthenticationScheme));

app.MapGet("hello", () => Results.Ok("Hello World"))
    .RequireAuthorization();

app.MapHealthChecks("health");

await app.RunAsync();
