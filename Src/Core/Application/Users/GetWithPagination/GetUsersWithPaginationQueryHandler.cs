using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernal.Primitives;

namespace Application.Users.GetWithPagination;

internal sealed class GetUsersWithPaginationQueryHandler
    : IQueryHandler<GetUsersWithPaginationQuery, List<UserPagedResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersWithPaginationQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<List<UserPagedResponse>>> Handle(GetUsersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        (IEnumerable<User> users, int totalCount) = await _userRepository.GetListWithPagingAsync(
            request.Input.Keyword,
            request.Input.PageIndex,
            request.Input.PageSize,
            cancellationToken);

        return PagedResult<UserPagedResponse>.Create(
            users.Select(u => new UserPagedResponse(u.Id, u.Username.Value, u.Email.Value)).ToList(),
            totalCount,
            request.Input.PageIndex,
            request.Input.PageSize);
    }
}
