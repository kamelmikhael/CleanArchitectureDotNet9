using Mapster.Models;
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
        , string? type = null
        , Error[]? errors = null)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Can't convert success result to problem details");
        }

        return Results.Problem(
            statusCode: status ?? StatusCodes.Status400BadRequest,
            title: title ?? "Bad Request",
            type: type ?? "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            extensions: new Dictionary<string, object?>
            {
                { "errors", errors ?? result.Errors }
            });
    }

    public static IResult ToProblemDetails<T>(
        this Result<T> result
        , string? title = null
        , int? status = null
        , string? type = null
        , Error[]? errors = null)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Can't convert success result to problem details");
        }

        return Results.Problem(
            statusCode: status ?? StatusCodes.Status400BadRequest,
            title: title ?? "Bad Request",
            type: type ?? "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            extensions: new Dictionary<string, object?>
            {
                { "errors", errors ?? result.Errors }
            });
    }

    public static IResult Handle(
        this Result result)
    {
        if (result is { IsSuccess: true })
        {
            return result.HandleSuccess();
        }

        return result.HandleFailure();
    }

    public static IResult HandleSuccess(
        this Result result)
    {
        result.ClearErrors();

        return Results.Ok(result);
    }

    public static IResult HandleFailure(
        this Result result)
    {
        if (result.Errors.Any(x => x.Type == ErrorType.Validation))
        {
            return result.HandleValidation();
        }
        else if (result.Errors.Any(x => x.Type == ErrorType.NotFound))
        {
            return result.HandleNotFound();
        }

        return result.ToProblemDetails(
            "Bad Request",
            StatusCodes.Status400BadRequest,
            "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            [.. result.Errors.Where(x => x != Error.None)]);
    }

    public static IResult HandleNotFound(
        this Result result)
    {
        return result.ToProblemDetails(
            "Not Found",
            StatusCodes.Status404NotFound,
            "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            [.. result.Errors.Where(x => x.Type == ErrorType.NotFound)]);
    }

    public static IResult HandleValidation(
        this Result result)
    {
        var validationError = result.Errors.FirstOrDefault() as ValidationError;

        if (validationError?.Errors?.Length > 0)
        {
            return result.ToProblemDetails(
                "Validation Errors"
                , StatusCodes.Status400BadRequest
                , "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                , validationError?.Errors);
        }

        return result.ToProblemDetails(
            "Validation Errors"
            , StatusCodes.Status400BadRequest
            , "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            , [.. result.Errors.Where(x => x.Type == ErrorType.Validation)]);
    }

    public static IResult HandleNoContent(
        this Result result)
    {
        result.ClearErrors();

        return Results.NoContent();
    }
}
