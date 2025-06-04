using SharedKernal.Guards;
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
        => Result.Combine(
                Result.Ensure( // Ensure not empty
                    email,
                    (Check.NotEmpty(), EmailErrors.EmailEmpty),
                    (Check.ValidLength(UserConsts.MinEmailLength, UserConsts.MaxEmailLength), EmailErrors.EmailInvalidLength),
                    (Check.ValidEmailFormat(), EmailErrors.EmailInvalidFormat)
                )
            )
            .Map(e => new Email(e));
}