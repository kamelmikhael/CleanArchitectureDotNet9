using Application.Abstractions.Messaging;
using Domain.Users;
using Mapster;
using SharedKernal.Primitives;

namespace Application.Users.GetById;

public sealed class GetUserByIdQueryHandler(
    IUserRepository userRepository) : IQueryHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    public async Task<Result<GetUserByIdResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        User? user = await userRepository.FindAsync(query.Id, cancellationToken);

        if (user is null)
        {
            return Result.Failure<GetUserByIdResponse>(UserErrors.NotFound(query.Id));
        }

        return new GetUserByIdResponse(user.Email.Value,
            user.CreatedOnUtc,
            user.UpdatedOnUtc);
    }
}
