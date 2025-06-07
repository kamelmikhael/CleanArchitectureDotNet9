using Application.Abstractions.Messaging;

namespace Application.Users.GetById;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<GetUserByIdResponse>;
