using System.Diagnostics.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SharedKernal.Primitives;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Errors = [error];
    }

    protected internal Result(bool isSuccess, Error[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; set; }

    public bool IsFailure => !IsSuccess;

    public Error[] Errors { get; private set; } = [];

    public void ClearErrors() => Errors = [];

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value)
        => new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result Failure(Error[] errors) => new(false, errors);

    public static Result<TValue> Failure<TValue>(Error error)
        => new(default, false, error);

    public static Result<TValue> Failure<TValue>(Error[] errors)
        => new(default, false, errors);

     public static Result<TValue> Create<TValue>(TValue? value)
        => value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static Result<TValue> Ensure<TValue>(
        TValue value,
        Func<TValue, bool> predicate,
        Error error) => predicate(value) ? Success(value) : Failure<TValue>(error);

    public static Result<TValue> Ensure<TValue>(
        TValue value,
        params (Func<TValue, bool>, Error)[] functions)
    {
        var results = new List<Result<TValue>>();

        foreach ((Func<TValue, bool> predicate, Error error) in functions)
        {
            results.Add(Ensure(value, predicate, error));
        }

        return Combine(results.ToArray());
    }

    public static Result<TValue> Combine<TValue>(params Result<TValue>[] results)
    {
        if (results.Any(r => r.IsFailure))
        {
            return Failure<TValue>(
                results.SelectMany(r => r.Errors).Distinct().ToArray());
        }

        return Success(results[0].Value);
    }

    public static Result<(T1, T2)> Combine<T1, T2>(
        Result<T1> result1, Result<T2> result2)
    {
        if (result1.IsFailure)
        {
            return Failure<(T1, T2)>(result1.Errors);
        }

        if (result2.IsFailure)
        {
            return Failure<(T1, T2)>(result2.Errors);
        }

        return Success((result1.Value, result2.Value));
    }

    public static bool TryCheckErrors(
        out Error[] errors,
        params Result[] results)
    {
        errors = results
            .Where(r => r.IsFailure && r.Errors.Length > 0)
            .SelectMany(r => r.Errors.Where(err => err != Error.None))
            .ToArray();

        return errors.Length > 0;
    }
}
