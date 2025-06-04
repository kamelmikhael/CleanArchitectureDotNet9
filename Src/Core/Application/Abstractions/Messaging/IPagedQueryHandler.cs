using SharedKernal.Primitives;

namespace Application.Abstractions.Messaging;

public interface IPagedQueryHandler<in TQuery, TResponse>
    where TQuery : IPagedQuery<TResponse>
{
    Task<PagedResult<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}
