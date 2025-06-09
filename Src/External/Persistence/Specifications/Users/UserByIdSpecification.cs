using Domain.Users;
using System.Linq.Expressions;

namespace Persistence.Specifications.Users;

internal sealed class UserByIdSpecification : Specification<User>
{
    public UserByIdSpecification(Guid userId) 
        : base(user => user.Id == userId)
    {
        IsSplitQuery = true;
    }
}
