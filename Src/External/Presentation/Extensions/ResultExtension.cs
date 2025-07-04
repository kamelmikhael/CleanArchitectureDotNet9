using Microsoft.AspNetCore.Http;
using SharedKernal.Primitives;

namespace Presentation.Extensions;

public static class ResultExtension
{
    public static async Task<IResult> Match(
        this Task<Result> resultTask,
        Func<Result, IResult> onSuccess,
        Func<Result, IResult> onFailure)
    {
        Result result = await resultTask;

        return result.IsSuccess
            ? onSuccess(result)
            : onFailure(result);
    }

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

    public static IResult ToProblemDetails(
        this Result result
        , string? title = null
        , int? status = null
        , Error[]? errors = null)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Can't convert success result to problem details");
        }

        return Results.Problem(
            statusCode: status ?? StatusCodes.Status400BadRequest,
            title: title ?? "Bad Request",
            type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            extensions: new Dictionary<string, object?>
            {
                { "errors", errors ?? result.Errors }
            });
    }

    public static IResult ToProblemDetails<T>(
        this Result<T> result
        , string? title = null
        , int? status = null
        , Error[]? errors = null)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Can't convert success result to problem details");
        }

        return Results.Problem(
            statusCode: status ?? StatusCodes.Status400BadRequest,
            title: title ?? "Bad Request",
            type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            extensions: new Dictionary<string, object?>
            {
                { "errors", errors ?? result.Errors }
            });
    }
}
