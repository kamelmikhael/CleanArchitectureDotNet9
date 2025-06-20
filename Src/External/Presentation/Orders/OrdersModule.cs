using Application.Abstractions.Messaging;
using Application.Orders.Create;
using Application.Orders.GetById;
using Application.Orders.RemoveLineItem;
using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Routing;
using Presentation.Extensions;
using SharedKernal.Primitives;

namespace Presentation.Orders;

//[EnableRateLimiting("fixed")]
//[DisableRateLimiting]
public class OrdersModule : CarterModule
{
    public OrdersModule()
        : base("/orders")
    {
        WithTags("Orders");
        // RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/{orderId:guid}/{lineItemId:guid}", async (
            Guid orderId,
            Guid lineItemId,
            ICommandHandler<RemoveLineItemCommand> handler,
            CancellationToken cancellationToken)
            => await Result
                .Create(new RemoveLineItemCommand(new(orderId), new(lineItemId)))
                .Bind(query => handler.Handle(query, cancellationToken))
                .Match(
                    ResultsResponse.HandleSuccess,
                    ResultsResponse.HandleFailure)
                );

        app.MapGet("/{orderId:guid}", async (
            Guid orderId,
            IQueryHandler<GetOrderByIdQuery, GetOrderByIdResponse> handler,
            CancellationToken cancellationToken)
            => await Result
                .Create(new GetOrderByIdQuery(new(orderId)))
                .Bind(query => handler.Handle(query, cancellationToken))
                .Match(
                    ResultsResponse.HandleSuccess,
                    ResultsResponse.HandleFailure)
                );

        app.MapPost("/", async (
            CreateOrderRequest request,
            ICommandHandler<CreateOrderCommand> handler,
            CancellationToken cancellationToken)
            => await Result
                .Create(new CreateOrderCommand(new(request.CustomerId)))
                .Bind(query => handler.Handle(query, cancellationToken))
                .Match(
                    ResultsResponse.HandleSuccess,
                    ResultsResponse.HandleFailure)
                );
            //.RequireRateLimiting("fixed");
    }
}
