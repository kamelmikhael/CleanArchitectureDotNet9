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

    public static IResult Handle<T>(Result<T> result)
    {
        if ( result is { IsSuccess: true })
        {
            result.ClearErrors();

            return Results.Ok(result);
        }

        return HandleFailure(result);
    }

    public static IResult HandleFailure(Result result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            { IsFailure: true } => result.Errors.Any(x => x.Type == ErrorType.Validation)
                ? Results.BadRequest(
                    CreateProblemDetails(
                        "Validation Error",
                        StatusCodes.Status400BadRequest,
                        result.Errors))
                : Results.BadRequest(
                        CreateProblemDetails(
                            "Bad Request",
                            StatusCodes.Status400BadRequest,
                            result.Errors)),
            _ =>
                Results.BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        StatusCodes.Status400BadRequest,
                        result.Errors))

        };
    }

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error[]? errors = null)
        => new()
        {
            Title = title,
            //Type = error.Code,
            //Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}
