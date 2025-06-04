using Application.Abstractions.Messaging;
using SharedKernal.Primitives;

namespace Application.Users.GetWithPagination;

public sealed record GetUsersWithPaginationQuery(UserPagedRequest Input) : IPagedQuery<UserPagedResponse>;
