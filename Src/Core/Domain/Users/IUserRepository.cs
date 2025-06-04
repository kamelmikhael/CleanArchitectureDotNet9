using SharedKernal.Abstractions.Data;

namespace Domain.Users;

public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Checks if a user with the specified email exists.
    /// </summary>
    /// <param name="email">The email to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if a user with the specified email exists, otherwise false.</returns>
    Task<bool> IsEmailExistsAsync(Email email, CancellationToken cancellationToken = default);

    Task<User?> GetByUsernameAsync(UserName username, CancellationToken cancellationToken = default);

    Task<(IEnumerable<User>, int)> GetListWithPagingAsync(
        string? keyword, 
        int page, 
        int pageSize, 
        CancellationToken cancellationToken = default);
}
