using Application.Abstractions.Messaging;
using Domain.Customers;
using SharedKernal.Abstractions.Data;
using SharedKernal.Primitives;

namespace Application.Customers.Create;

public sealed class CreateCustomerCommandHandler(
    ICustomerRepository repository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateCustomerCommand>
{
    public async Task<Result> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(command.Email, command.Name);

        repository.Add(customer);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
