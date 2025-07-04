using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using SharedKernal.Primitives;

namespace Presentation;

internal static class ResultsResponse
{
    public static IResult Handle<T>(PagedResult<T> result)
    {
        if (result is { IsSuccess: true })
        {
            result.ClearErrors();

            return Results.Ok(result);
        }

        return HandleFailure(result);
    }

    public static IResult Handle(Result result)
    {
        if ( result is { IsSuccess: true })
        {
            result.ClearErrors();

            return Results.Ok(result);
        }

        return HandleFailure(result);
    }

    public static IResult HandleSuccess(Result result)
    {
        if (result is { IsFailure: true })
        {
            throw new InvalidOperationException();
        }

        result.ClearErrors();

        return Results.Ok(result);
    }

    public static IResult HandleFailure(Result result)
    {
        if (result.Errors.Any(x => x.Type == ErrorType.Validation))
        {
            return HandleValidationError(result);
        }
        else if (result.Errors.Any(x => x.Type == ErrorType.NotFound))
        {
            return HandleNotFoundError(result);
        }

        return result.ToProblemDetails(
            "Bad Request",
            StatusCodes.Status400BadRequest,
            [.. result.Errors.Where(x => x != Error.None)]);
    }

    private static IResult HandleNotFoundError(Result result)
    {
        return result.ToProblemDetails(
            "Not Found Errors",
            StatusCodes.Status404NotFound,
            [.. result.Errors.Where(x => x.Type == ErrorType.NotFound)]);
    }

    private static IResult HandleValidationError(Result result)
    {
        var validationError = result.Errors.FirstOrDefault() as ValidationError;

        if (validationError?.Errors?.Length > 0)
        {
            return result.ToProblemDetails(
                "Validation Errors"
                , StatusCodes.Status400BadRequest
                , validationError?.Errors);
        }

        return result.ToProblemDetails(
            "Validation Errors"
            , StatusCodes.Status400BadRequest
            , [.. result.Errors.Where(x => x.Type == ErrorType.Validation)]);
    }
}
