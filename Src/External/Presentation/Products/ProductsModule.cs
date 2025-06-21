using Application.Abstractions.Messaging;
using Application.Products.Create;
using Application.Products.GetList;
using Application.Products.Delete;
using Application.Products.Update;
using Carter;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Presentation.Extensions;
using SharedKernal.Primitives;
using Microsoft.AspNetCore.Mvc;

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
            CreateProductRequest requset
            , ICommandHandler<CreateProduct.Command> handler
            , CancellationToken cancellationToken) => await Result
                .Create(requset.Adapt<CreateProduct.Command>())
                .Bind(command => handler.Handle(command, cancellationToken))
                .Match(
                    ResultsResponse.HandleSuccess,
                    ResultsResponse.HandleFailure));

        app.MapGet("/", async (
            IQueryHandler<GetProductList.Query, List<GetProductList.Response>?> handler
            , CancellationToken cancellationToken) => await Result
                .Create(new GetProductList.Query())
                .Bind(query => handler.Handle(query, cancellationToken))
                .Match(
                    ResultsResponse.HandleSuccess,
                    ResultsResponse.HandleFailure));

        app.MapDelete("/{id:guid}", async (
            Guid id
            , ICommandHandler<DeleteProduct.Command> handler
            , CancellationToken cancellationToken) => await Result
                .Create(new DeleteProduct.Command(id))
                .Bind(command => handler.Handle(command, cancellationToken))
                .Match(
                    ResultsResponse.HandleSuccess,
                    ResultsResponse.HandleFailure));

        app.MapPut("/{id:guid}", async (
            Guid id
            , [FromBody] UpdateProductRequest request
            , ICommandHandler<UpdateProduct.Command> handler
            , CancellationToken cancellationToken) => await Result
                .Create(new UpdateProduct.Command(id, request.Name))
                .Bind(command => handler.Handle(command, cancellationToken))
                .Match(
                    ResultsResponse.HandleSuccess,
                    ResultsResponse.HandleFailure));
    }
}
