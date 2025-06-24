using System.Transactions;
using Application.Abstractions.Messaging;
using SharedKernal.Abstractions.Data;
using SharedKernal.Primitives;

namespace Application.Abstractions.Behaviors;

internal static class UnitOfWorkDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IUnitOfWork unitOfWork)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            Result<TResponse> response = await innerHandler.Handle(command, cancellationToken);

            await unitOfWork.SaveChangesAsync(default);

            transactionScope.Complete();

            return response;
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IUnitOfWork unitOfWork)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            Result response = await innerHandler.Handle(command, cancellationToken);

            await unitOfWork.SaveChangesAsync(default);

            transactionScope.Complete();

            return response;
        }
    }
}
