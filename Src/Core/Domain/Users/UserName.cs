using SharedKernal.Primitives;
using SharedKernal.Guards;

namespace Domain.Users;

public sealed record UserName
{
    public string Value { get; private set; }

    private UserName()
    { }

    private UserName(string value)
    {
        Value = value;
    }

    public static Result<UserName> Create(string username)
        => Result.Combine(
                Result.Ensure(
                    username,
                    (Check.NotEmpty(), UserNameErrors.UserNameEmpty),
                    (Check.ValidLength(UserConsts.MinUserNameLength, UserConsts.MaxUserNameLength), UserNameErrors.UserNameInvalidLength)
                )
            )
            .Map(n => new UserName(n));

    public static explicit operator string(UserName userName) => userName.Value;
}
