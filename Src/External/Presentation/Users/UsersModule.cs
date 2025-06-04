using Application.Abstractions.Messaging;
using Application.Users.Login;
using Application.Users.Register;
using Carter;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernal.Primitives;

namespace Presentation.Users;

public class UsersModule : CarterModule
{
    public UsersModule()
        : base("/users")
    {
        WithTags("Users");
        // RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/register", async (
            RegisterUserRequest request, 
            ICommandHandler<RegisterUserCommand, Guid> handler, 
            CancellationToken cancellationToken) =>
        {
            RegisterUserCommand command = request.Adapt<RegisterUserCommand>();

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return ResultsResponse.Handle(result);
        });

        app.MapPost("/login", async (
            LoginUserRequest request,
            ICommandHandler<LoginUserCommand, string> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginUserCommand(request.Username);

            Result<string> result = await handler.Handle(command, cancellationToken);

            return ResultsResponse.Handle(result);
        });

    }

    public sealed record RegisterUserRequest(
        string Username, 
        string Email, 
        string Password);

    public sealed record LoginUserRequest(string Username);
}