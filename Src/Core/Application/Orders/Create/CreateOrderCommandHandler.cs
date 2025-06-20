using Application.Abstractions.Messaging;
using Domain.Customers;
using Domain.Orders;
using Rebus.Bus;
using SharedKernal.Abstractions;
using SharedKernal.Abstractions.Data;
using SharedKernal.Primitives;

namespace Application.Orders.Create;

public sealed class CreateOrderCommandHandler(
    ICustomerRepository customerRepository
    , IOrderRepository orderRepository
    //, IBus bus
    , IUnitOfWork unitOfWork) : ICommandHandler<CreateOrderCommand>
{
    public async Task<Result> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        Customer? customer = await customerRepository
            .FindAsync(command.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(command.CustomerId));
        }

        var order = Order.Create(command.CustomerId);

        orderRepository.Add(order);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        //await bus.Send(new OrderCreatedEvent(order.Id.Value));

        return Result.Success();
    }
}
