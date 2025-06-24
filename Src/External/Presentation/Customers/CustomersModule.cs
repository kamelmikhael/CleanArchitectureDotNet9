using Application.Abstractions.Messaging;
using Application.Customers.Create;
using Carter;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Presentation.Extensions;
using SharedKernal.Primitives;

namespace Presentation.Customers;

public class CustomersModule : CarterModule
{
    public CustomersModule()
        : base("/customers")
    {
        WithTags("Customers");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateCustomerRequest request
            , ICommandHandler<CreateCustomerCommand> handler
            , CancellationToken cancellationToken) => await Result
                .Create(request.Adapt<CreateCustomerCommand>())
                .Bind(query => handler.Handle(query, cancellationToken))
                .Match(
                    ResultsResponse.HandleSuccess,
                    ResultsResponse.HandleFailure)
                );
    }
}
