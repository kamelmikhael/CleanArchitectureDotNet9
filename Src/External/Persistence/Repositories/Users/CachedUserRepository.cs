using Domain.Users;
using SharedKernal.Abstraction.Caching;

namespace Persistence.Repositories;

public sealed class CachedUserRepository(
    ApplicationDbContext context,
    IUserRepository decorated,
    ICacheService cacheService) : Repository<User>(context), IUserRepository
{
    public override Task<User?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    => cacheService.GetOrCreateAsync(
            $"User-{id}",
            async () => await decorated.FindAsync(id, cancellationToken),
            cancellationToken);

    public Task<User?> GetByUsernameAsync(UserName username, CancellationToken cancellationToken = default)
    => cacheService.GetOrCreateAsync(
            $"User-{username.Value}",
            async () => await decorated.GetByUsernameAsync(username, cancellationToken),
            cancellationToken);

    public Task<(IEnumerable<User>, int)> GetListWithPagingAsync(
        string? keyword, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return decorated.GetListWithPagingAsync(keyword, page, pageSize, cancellationToken);
    }

    public Task<bool> IsEmailExistsAsync(Email email, CancellationToken cancellationToken = default)
    {
       return decorated.IsEmailExistsAsync(email, cancellationToken);
    }
}
