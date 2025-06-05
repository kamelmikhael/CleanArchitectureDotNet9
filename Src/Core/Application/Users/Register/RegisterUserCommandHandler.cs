using Application.Abstractions.Authentication;
using SharedKernal.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernal.Primitives;
using SharedKernal.Abstraction.EventBus;

namespace Application.Users.Register;

internal sealed class RegisterUserCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository repository, 
    IPasswordHasher passwordHasher
    //,IEventBusService eventBus
    ) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        Result<UserName> userNameResult = UserName.Create(command.Username);

        Result<Email> emailResult = Email.Create(command.Email);

        if (Result.ContainsErrors(out Error[] errors, userNameResult, emailResult))
        {
            return Result.Failure<Guid>(errors);
        }

        bool isEmailExists = await repository.IsEmailExistsAsync(emailResult.Value, cancellationToken);

        Result<User> userResult = User.Create(Guid.NewGuid()
            , userNameResult.Value
            , emailResult.Value
            , passwordHasher.Hash(command.Password),
            isEmailExists);

        if (userResult.IsFailure)
        {
            return Result.Failure<Guid>(userResult.Errors);
        }

        User user = userResult.Value;

        user.Raise(new UserRegisteredDomainEvent(user.Id));

        repository.Add(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        //await eventBus.PublishAsync(new UserRegisteredEventBus 
        //{ 
        //    Id = user.Id, 
        //    Username = user.Username.Value,
        //}, cancellationToken);

        return user.Id;
    }
}
