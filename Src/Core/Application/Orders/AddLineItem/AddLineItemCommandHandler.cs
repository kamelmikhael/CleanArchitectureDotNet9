using Application.Abstractions.Messaging;
using Application.Orders.Services;
using Domain.Orders;
using Domain.Products;
using SharedKernal.Abstraction.Data;
using SharedKernal.Primitives;

namespace Application.Orders.AddLineItem;

internal sealed class AddLineItemCommandHandler(
    IGetOrderByIdService orderService
    , IRepository<Product, ProductId> productRepository) : ICommandHandler<AddLineItemCommand>
{
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

        return Result.Success();
    }
}
