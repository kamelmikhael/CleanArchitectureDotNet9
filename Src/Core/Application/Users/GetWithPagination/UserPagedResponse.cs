namespace Application.Users.GetWithPagination;

public sealed record UserPagedResponse(Guid Id, string Username, string Email);
