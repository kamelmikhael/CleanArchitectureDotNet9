using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        if (result is { IsSuccess: true })
        {
            throw new InvalidOperationException();
        }

        if (result.Errors.Any(x => x.Type == ErrorType.Validation))
        {
            return HandleValidationError(result);
        }
        else if (result.Errors.Any(x => x.Type == ErrorType.NotFound))
        {
            return HandleNotFoundError(result);
        }

        return Results.BadRequest(
                        CreateProblemDetails(
                            "Bad Request",
                            StatusCodes.Status400BadRequest,
                            [.. result.Errors.Where(x => x != Error.None)]));
    }

    private static IResult HandleNotFoundError(Result result)
    {
        return Results.NotFound(
                                CreateProblemDetails(
                                    "Not Found Errors",
                                    StatusCodes.Status404NotFound,
                                    [.. result.Errors.Where(x => x.Type == ErrorType.NotFound)]));
    }

    private static IResult HandleValidationError(Result result)
    {
        var validationError = result.Errors.FirstOrDefault() as ValidationError;

        if (validationError?.Errors?.Length > 0)
        {
            return Results.BadRequest(
                    CreateProblemDetails(
                        "Validation Errors",
                        StatusCodes.Status400BadRequest,
                        errors: validationError?.Errors));
        }

        return Results.BadRequest(
                    CreateProblemDetails(
                        "Validation Errors",
                        StatusCodes.Status400BadRequest,
                        [.. result.Errors.Where(x => x.Type == ErrorType.Validation)]));
    }

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error[]? errors = null)
        => new()
        {
            Title = title,
            // Type = errors?.FirstOrDefault()?.Code,
            // Detail = errors?.FirstOrDefault()?.Description,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}
