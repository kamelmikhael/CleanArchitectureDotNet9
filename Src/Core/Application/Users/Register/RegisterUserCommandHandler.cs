using Application.Abstractions.Authentication;
using SharedKernal.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernal.Primitives;

namespace Application.Users.Register;

internal sealed class RegisterUserCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository repository, 
    IPasswordHasher passwordHasher) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var userNameResult = UserName.Create(command.Username);

        if (userNameResult.IsFailure) return Result.Failure<Guid>(userNameResult.Error);

        var emailResult = Email.Create(command.Email);

        if (emailResult.IsFailure) return Result.Failure<Guid>(emailResult.Error);

        var isEmailExists = await repository.IsEmailExistsAsync(emailResult.Value, cancellationToken);

        var userResult = User.Create(Guid.NewGuid()
            , userNameResult.Value
            , emailResult.Value
            , passwordHasher.Hash(command.Password),
            isEmailExists);

        if (userResult.IsFailure) return Result.Failure<Guid>(userResult.Error);

        var user = userResult.Value;

        user.Raise(new UserRegisteredDomainEvent(user.Id));

        repository.Add(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
