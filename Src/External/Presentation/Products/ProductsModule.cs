using Carter;
using Microsoft.AspNetCore.Routing;
using Application.Products.Create;
using Application.Abstractions.Messaging;
using SharedKernal.Primitives;
using Mapster;
using Presentation.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Presentation.Products;

public class ProductsModule : CarterModule
{
    public ProductsModule()
        : base("/products")
    {
        WithTags("Products");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            CreateProduct.Request requset
            , ICommandHandler<CreateProduct.Command> handler
            , CancellationToken cancellationToken) => await Result
                .Create(requset.Adapt<CreateProduct.Command>())
                .Bind(command => handler.Handle(command, cancellationToken))
                .Match(
                    ResultsResponse.HandleSuccess,
                    ResultsResponse.HandleFailure));
    }
}
