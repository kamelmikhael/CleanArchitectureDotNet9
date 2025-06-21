using Application.Abstractions.Messaging;
using Domain.Products;
using SharedKernal.Abstractions;
using SharedKernal.Abstractions.Data;
using SharedKernal.Primitives;

namespace Application.Products.Update;

public sealed class UpdateProduct
{
    public sealed record Command(Guid Id, string Name) : ICommand;

    internal sealed class Handler(
        IRepository<Product, ProductId> repository
        , IUnitOfWork unitOfWork
        , IEventPublisher publisher) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var productId = new ProductId(command.Id);
            Product? product = await repository
                .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

            if (product is null)
            {
                return Result.Failure(ProductErrors.NotFound(productId));
            }

            product.SetName(command.Name);

            repository.Update(product);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publisher.PublishAsync(new ProductUpdatedEvent(command.Id), cancellationToken);

            return Result.Success();
        }
    }
}
