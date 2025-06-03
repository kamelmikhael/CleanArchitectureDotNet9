using Domain.Users;

namespace Persistence.Specifications.Users;

internal sealed class UserUsernameSpecification : Specification<User>
{
    public UserUsernameSpecification(UserName username)
        : base(user => user.Username == username)
    { }
}

