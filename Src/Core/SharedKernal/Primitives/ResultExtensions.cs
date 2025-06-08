﻿namespace SharedKernal.Primitives;

public static class ResultExtensions
{
    public static Result<TOut> Map<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> mappinngFunc)
    {
        return result.IsSuccess 
            ? Result.Success(mappinngFunc(result.Value)) 
            : Result.Failure<TOut>(result.Errors);
    }

    public static async Task<Result> Bind<TIn>(
        this Result<TIn> result,
        Func<TIn, Task<Result>> func)
    {
        if (result.IsFailure)
        {
            return Result.Failure(result.Errors);
        }

        return await func(result.Value);
    }

    public static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> func)
    {
        if (result.IsFailure)
        {
            return Result.Failure<TOut>(result.Errors);
        }

        return await func(result.Value);
    }
}
