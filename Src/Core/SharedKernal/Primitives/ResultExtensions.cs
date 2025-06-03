namespace SharedKernal.Primitives;

public static class ResultExtensions
{
    public static Result<T> Ensure<T>(
        this Result<T> result,
        Func<T, bool> predicate,
        Error error)
    {
        if(result.IsFailure) return result;

        return predicate(result.Value) ? result : Result.Failure<T>(error);
    }

    public static Result<TOut> Map<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> mappinngFunc)
    {
        return result.IsSuccess 
            ? Result.Success(mappinngFunc(result.Value)) 
            : Result.Failure<TOut>(result.Error);
    }
}
