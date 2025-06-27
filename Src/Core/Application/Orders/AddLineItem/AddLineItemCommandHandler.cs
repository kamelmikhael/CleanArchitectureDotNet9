using Application.Abstractions.Messaging;
using Application.Orders.Services;
using Domain.Orders;
using Domain.Products;
using SharedKernal.Abstraction.Data;
using SharedKernal.Abstractions.Data;
using SharedKernal.Primitives;

namespace Application.Orders.AddLineItem;

internal sealed class AddLineItemCommandHandler(
    IGetOrderByIdService orderService
    , IRepository<Product, ProductId> productRepository
    , IUnitOfWork unitOfWork) : ICommandHandler<AddLineItemCommand>
{
    //public async Task<Result> Handle(AddLineItemCommand command, CancellationToken cancellationToken) =>
    //    await Result
    //        .Combine(
    //            Result.Create(await orderService.ExecuteAsync(command.OrderId, cancellationToken)),
    //            Result.Create(await productRepository.FindAsync(command.ProductId, cancellationToken)))
    //        .Bind(t => t.Item1.AddLineItem(
    //            t.Item2.Id
    //            , new Money(t.Item2.Price.Currency, t.Item2.Price.Amount)
    //            , command.Qty))
    //        .Tap(() => unitOfWork.SaveChangesAsync(cancellationToken));

    public async Task<Result> Handle(AddLineItemCommand command, CancellationToken cancellationToken)
    {
        Order? order = await orderService.ExecuteAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound(command.OrderId));
        }

        Product? product = await productRepository.FindAsync(command.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound(command.ProductId));
        }

        order.AddLineItem(product.Id
            , new Money(product.Price.Currency, product.Price.Amount)
            , command.Qty);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
