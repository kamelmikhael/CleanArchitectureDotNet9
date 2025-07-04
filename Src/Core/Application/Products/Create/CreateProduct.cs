using Application.Abstractions.Messaging;
using Domain.Products;
using FluentValidation;
using SharedKernal.Abstraction.Data;
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
        string Sku) : ICommand<Guid>;

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Currency).NotEmpty();
            RuleFor(x => x.Amount).Must(x => x > 0 && x < 1000);
            RuleFor(x => x.Sku).NotEmpty().Length(Sku.DefaultLength, Sku.DefaultLength);
        }
    }

    internal sealed class Handler(
        IRepository<Product, ProductId> repository,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher publisher) : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command command, CancellationToken cancellationToken)
        {
            var money = new Money(command.Currency, command.Amount);
            var sku = Sku.Create(command.Sku);

            var product = Product.Create(command.Name, money, sku);

            repository.Add(product);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publisher.PublishAsync(new ProductCreatedDomainEvent(product.Id.Value), cancellationToken);

            return product.Id.Value;
        }
    }
}
