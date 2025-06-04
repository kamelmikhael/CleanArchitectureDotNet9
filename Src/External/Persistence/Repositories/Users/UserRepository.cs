using Application.Users;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;
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

    public async Task<(IEnumerable<User>, int)> GetListWithPagingAsync(
        string? keyword, 
        int page, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        IQueryable<User> query = _dbSet
            //.WhereIf(
            //    !string.IsNullOrWhiteSpace(keyword), 
            //    x => x.Username.Value.Contains(keyword!) || x.Email.Value.Contains(keyword!))
            .AsNoTracking()
            .AsQueryable();

        return (
            await query.Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken),
            await query.CountAsync(cancellationToken)
        );
    }
}
