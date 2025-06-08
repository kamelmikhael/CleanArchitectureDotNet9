using Microsoft.AspNetCore.Http;
using SharedKernal.Primitives;

namespace Presentation.Extensions;

public static class ResultExtension
{
    public static async Task<IResult> Match<T>(
        this Task<Result<T>> resultTask,
        Func<Result<T>, IResult> onSuccess,
        Func<Result<T>, IResult> onFailure)
    {
        Result<T> result = await resultTask;

        return result.IsSuccess
            ? onSuccess(result)
            : onFailure(result);
    }
}
