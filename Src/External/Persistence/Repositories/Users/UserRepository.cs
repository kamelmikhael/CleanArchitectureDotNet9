﻿using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;
using Persistence.Specifications.Users;

namespace Persistence.Repositories;

internal sealed class UserRepository(ApplicationDbContext context) 
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
        UserName userName = UserName.Create(keyword!).Value;
        Email email = Email.Create(keyword!).Value;

        IQueryable<User> query = _dbSet
            .WhereIf(
                !string.IsNullOrWhiteSpace(keyword),
                x => x.Username == userName || x.Email == email)
            .AsNoTracking()
            .AsQueryable();

        return (
            await query.OrderBy(x => x.Id)
                       .Skip(page * pageSize)
                       .Take(pageSize)
                       .ToListAsync(cancellationToken),
            await query.CountAsync(cancellationToken)
        );
    }
}
