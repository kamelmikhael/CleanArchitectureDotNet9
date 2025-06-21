using Application.Abstractions.Messaging;
using Domain.Products;
using FluentValidation;
using SharedKernal.Abstractions;
using SharedKernal.Abstractions.Data;
using SharedKernal.Primitives;

namespace Application.Products.Create;

public sealed class CreateProduct
{
    public sealed record Command(
        string Name,
        string Currency,
        decimal Amount,
        string Sku) : ICommand;

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Currency).NotEmpty();
            RuleFor(x => x.Amount).Must(x => x > 0 && x < 1000);
            RuleFor(x => x.Sku).NotEmpty();
        }
    }

    internal sealed class Handler(
        IRepository<Product, ProductId> repository,
        IUnitOfWork unitOfWork,
        IEventPublisher publisher) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var money = new Money(command.Currency, command.Amount);
            var sku = Sku.Create(command.Sku);

            var product = Product.Create(command.Name, money, sku);

            repository.Add(product);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publisher.PublishAsync(new ProductCreatedEvent(product.Id.Value), cancellationToken);

            return Result.Success(product.Id.Value);
        }
    }
}
