using Domain.Users;

namespace Application.Abstractions.Authentication;

public interface IJwtTokenProvider
{
    Task<string> GenerateAsync(User user);
}
