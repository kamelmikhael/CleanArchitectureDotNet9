using SharedKernal.Primitives;

namespace Domain.Users;

public static class EmailErrors
{
    public static Error EmailEmpty
        => Error.Validation("Email.Empty", "Email cannot be empty.");

    public static Error EmailInvalidLength
        => Error.Validation("Email.InvalidLength",
                $"Email length must between {UserConsts.MinEmailLength} and {UserConsts.MaxEmailLength} characters.");

    public static Error EmailInvalidFormat
        => Error.Validation("Email.InvalidFormat", "Email Invalid Format.");
}