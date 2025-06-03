using SharedKernal.Primitives;

namespace Domain.Users;

public static class UserNameErrors
{
    public static Error UserNameEmpty
        => Error.Validation("UserName.Empty", "Username cannot be empty.");

    public static Error UserNameInvalidLength
        => Error.Validation("UserName.InvalidLength",
                $"Username length must between {UserConsts.MinUserNameLength} and {UserConsts.MaxUserNameLength} characters.");
}
