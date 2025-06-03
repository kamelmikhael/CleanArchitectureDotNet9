using SharedKernal.Primitives;

namespace Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "Users.NotFound",
        $"The user with the Id = '{userId}' was not found");

    public static Error Unauthorized => Error.Failure(
        "Users.Unauthorized",
        "You are not authorized to perform this action.");

    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Users.NotFoundByEmail",
        "The user with the specified email was not found");

    public static readonly Error InvalidCredentials = Error.NotFound(
        "Users.InvalidCredentials",
        "Invalid Credentials");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "The provided email is not unique");

    public static Error InvalidEmailFormat(string email)
        => Error.Validation("User.InvalidEmailFormat", $"The email '{email}' is not in a valid format.");

    public static Error PasswordTooWeak
        => Error.Validation("User.PasswordTooWeak", "The provided password does not meet the required strength criteria.");
}
