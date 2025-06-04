using System.Diagnostics.CodeAnalysis;

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
        Errors = new[] { error };
    }

    protected internal Result(bool isSuccess, Error[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; set; }

    public bool IsFailure => !IsSuccess;

    public Error[] Errors { get; } = [];

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

    public static bool ContainsErrors(
        out Error[] errors,
        params Result[] results)
    {
        errors = results
            .Where(r => r.IsFailure && r.Errors.Length > 0)
            .SelectMany(r => r.Errors.Where(err => !string.IsNullOrEmpty(err.Code)))
            .ToArray();

        return errors.Length > 0;
    }
}

public class Result<TValue> : Result
{
    public TValue Value { get; }

    protected internal Result(TValue value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    protected internal Result(TValue value, bool isSuccess, Error[] errors)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    public static implicit operator Result<TValue>(TValue value) => Create(value);
}
