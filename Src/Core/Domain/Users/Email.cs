using SharedKernal.Primitives;

namespace Domain.Users;

public sealed class Email : ValueObject
{
    public string Value { get; private set; }

    private Email(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<Email> Create(string email)
        => Result
            .Create(email)
            .Ensure(
                e => !string.IsNullOrWhiteSpace(e),
                EmailErrors.EmailEmpty)
            .Ensure(
                e => e.Length <= UserConsts.MaxEmailLength && e.Length >= UserConsts.MinEmailLength,
                EmailErrors.EmailInvalidLength)
            .Ensure(
                e => e.Split('@').Length == 2,
                EmailErrors.EmailInvalidFormat)
            .Map(e => new Email(e));
}