using Application.Abstractions.Messaging;
using Application.Users.GetById;
using Application.Users.GetWithPagination;
using Application.Users.Login;
using Application.Users.Register;
using Carter;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using SharedKernal.Primitives;
using Presentation.Extensions;
using Microsoft.AspNetCore.Http;

namespace Presentation.EndPoints;

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
            "/search", async (
            UserPagedRequest input,
            IPagedQueryHandler<GetUsersWithPaginationQuery, UserPagedResponse> handler,
            CancellationToken cancellationToken) =>
            {
                GetUsersWithPaginationQuery query = new(input);

                PagedResult<UserPagedResponse> result = await handler.Handle(query, cancellationToken);

                return ResultsResponse.Handle(result);
            });

        app.MapPost(
            "/register", async (
            RegisterUserRequest request,
            ICommandHandler<RegisterUserCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            RegisterUserCommand command = request.Adapt<RegisterUserCommand>();

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return ResultsResponse.Handle(result);
        }); //.AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();

        app.MapPost("/login", async (
            LoginUserRequest request,
            ICommandHandler<LoginUserCommand, string> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginUserCommand(request.Username);

            Result<string> result = await handler.Handle(command, cancellationToken);

            return ResultsResponse.Handle(result);
        });

        app.MapGet("/{userId:guid}", async (
            Guid userId,
            IQueryHandler<GetUserByIdQuery, GetUserByIdResponse> handler,
            CancellationToken cancellationToken) 
            => await Result
                .Create(new GetUserByIdQuery(userId))
                .Bind(query => handler.Handle(query, cancellationToken))
                .Match(
                    ResultsResponse.HandleSuccess,
                    ResultsResponse.HandleFailure)
                );
    }

    public sealed record RegisterUserRequest(
        string Username, 
        string Email, 
        string Password);

    public sealed record LoginUserRequest(string Username);
}
