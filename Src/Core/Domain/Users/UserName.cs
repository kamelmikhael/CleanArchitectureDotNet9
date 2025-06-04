using SharedKernal.Primitives;
using SharedKernal.Guards;

namespace Domain.Users;

public sealed class UserName : ValueObject
{
    public string Value { get; private set; }

    private UserName(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
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
}
