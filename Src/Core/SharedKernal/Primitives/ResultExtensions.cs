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
}
