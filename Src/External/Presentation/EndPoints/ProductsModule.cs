using Application.Abstractions.Messaging;
using Application.Products.Create;
using Application.Products.Delete;
using Application.Products.GetList;
using Application.Products.GetPagedList;
using Application.Products.Update;
using Carter;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Extensions;
using SharedKernal.Primitives;

namespace Presentation.EndPoints;

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
            [FromBody] CreateProductRequest requset
            , ICommandHandler<CreateProduct.Command, Guid> handler
            , CancellationToken cancellationToken) => await Result
                .Create(requset.Adapt<CreateProduct.Command>())
                .Bind(command => handler.Handle(command, cancellationToken))
                .Match(
                    (result) => result.HandleSuccess(),
                    (result) => result.HandleFailure()
                )
        );

        app.MapGet("/", async (
            IQueryHandler<GetProductList.Query, List<GetProductListResponse>?> handler
            , CancellationToken cancellationToken) => await Result
                .Create(new GetProductList.Query())
                .Bind(query => handler.Handle(query, cancellationToken))
                .Match(
                    (result) => result.HandleSuccess(),
                    (result) => result.HandleFailure()
                )
        ).MapToApiVersion(1);

        app.MapGet("/", async (
            IQueryHandler<GetProductList.Query, List<GetProductListResponse>?> handler
            , CancellationToken cancellationToken) => await Result
                .Create(new GetProductList.Query())
                .Bind(query => handler.Handle(query, cancellationToken))
                .Match(
                    (result) => result.HandleSuccess(),
                    (result) => result.HandleFailure()
                )
        ).MapToApiVersion(2);

        app.MapPost("/search", async (
            ProductsPagedRequest request
            , IQueryHandler<GetProductsPaged.Query, List<ProductsPagedResponse>> handler
            , CancellationToken cancellationToken) =>
        {
            var query = new GetProductsPaged.Query(request);

            Result<List<ProductsPagedResponse>> response = await handler.Handle(query, cancellationToken);

            return response.Handle();
        });

        app.MapDelete("/{id:guid}", async (
            Guid id
            , ICommandHandler<DeleteProduct.Command> handler
            , CancellationToken cancellationToken) => await Result
                .Create(new DeleteProduct.Command(id))
                .Bind(command => handler.Handle(command, cancellationToken))
                .Match(
                    (result) => result.HandleSuccess(),
                    (result) => result.HandleFailure()
                )
        );

        app.MapPut("/{id:guid}", async (
            Guid id
            , [FromBody] UpdateProductRequest request
            , ICommandHandler<UpdateProduct.Command> handler
            , CancellationToken cancellationToken) => await Result
                .Create(new UpdateProduct.Command(id, request.Name))
                .Bind(command => handler.Handle(command, cancellationToken))
                .Match(
                    (result) => result.HandleSuccess(),
                    (result) => result.HandleFailure()
                )
        );
    }
}
