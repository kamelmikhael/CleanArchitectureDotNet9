using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Authentication.Permissions;

public class PermissionService(ApplicationDbContext dbContext) : IPermissionService
{
    public async Task<HashSet<string>> GetPermissionsAsync(Guid userId)
    {
        var roles = await dbContext
            .Set<User>()
            .Include(x => x.Roles)
                .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == userId)
            .Select(x => x.Roles)
            .ToArrayAsync();

        return roles
            .SelectMany(x => x)
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToHashSet();
    }
}
