namespace SharedKernal.Primitives;

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

    public static async Task<Result> Bind(
        this Result result,
        Func<Task<Result>> func)
    {
        if (result.IsFailure)
        {
            return Result.Failure(result.Errors);
        }

        return await func();
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

    public static Result Bind(
        this Result result,
        Func<Result> func)
    {
        if (result.IsFailure)
        {
            return Result.Failure(result.Errors);
        }

        return func();
    }

    public static Result Bind<TIn>(
        this Result<TIn> result,
        Func<TIn, Result> func)
    {
        if (result.IsFailure)
        {
            return Result.Failure(result.Errors);
        }

        return func(result.Value);
    }

    public static Result<TOut> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Result<TOut>> func)
    {
        if (result.IsFailure)
        {
            return Result.Failure<TOut>(result.Errors);
        }

        return func(result.Value);
    }

    public static Result<TIn> Tap<TIn>(
        this Result<TIn> result,
        Action<TIn> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }

    public static async Task<Result<TIn>> Tap<TIn>(
        this Result<TIn> result,
        Func<Task> func)
    {
        if (result.IsSuccess)
        {
            await func();
        }

        return result;
    }

    public static async Task<Result<TIn>> Tap<TIn>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, Task> func)
    {
        Result<TIn> result = await resultTask;

        if (result.IsSuccess)
        {
            await func(result.Value);
        }

        return result;
    }

    public static Result<TOut> TryCatch<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> func,
        Error error)
    {
        try
        {
            return result.IsSuccess
                ? func(result.Value)
                : Result.Failure<TOut>(result.Errors);
        }
        catch
        {
            return Result.Failure<TOut>(error);
        }
    }

    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess, 
        Func<Error[], TOut> onFailure)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result.Errors);
    }
}
