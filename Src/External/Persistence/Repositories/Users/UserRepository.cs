using Application.Users;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Specifications.Users;

namespace Persistence.Repositories;

public sealed class UserRepository(ApplicationDbContext context) 
    : Repository<User>(context), IUserRepository
{
    public Task<bool> IsEmailExistsAsync(Email email, CancellationToken cancellationToken = default)
    {
        return _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public Task<User?> GetByUsernameAsync(UserName username, CancellationToken cancellationToken = default)
    {
        return ApplySpecification(new UserUsernameSpecification(username))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
