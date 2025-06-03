using SharedKernal.Primitives;

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
        => Result
            .Create(username)
            .Ensure(
                n => !string.IsNullOrWhiteSpace(n),
                UserNameErrors.UserNameEmpty)
            .Ensure(
                n => username.Length <= UserConsts.MaxUserNameLength && username.Length >= UserConsts.MinUserNameLength,
                UserNameErrors.UserNameInvalidLength)
            .Map(n => new UserName(n));
}
