using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernal.Primitives;

namespace Application.Users.Login;

internal sealed class LoginUserCommandHandler(
    IUserRepository repository,
    IJwtTokenProvider tokenProvider) : ICommandHandler<LoginUserCommand, string>
{
    public async Task<Result<string>> Handle(
        LoginUserCommand command,
        CancellationToken cancellationToken)
    {
        Result<UserName> userNameResult = UserName.Create(command.Username);

        User? user = await repository.GetByUsernameAsync(userNameResult.Value, cancellationToken);

        if (user is null)
        {
            return Result.Failure<string>(UserErrors.InvalidCredentials);
        }

        return await tokenProvider.GenerateAsync(user);
    }
}
