namespace Application.Users.GetById;

public sealed record GetUserByIdResponse(string Email, DateTime CreatedOnUtc, DateTime? UpdatedOnUtc);
