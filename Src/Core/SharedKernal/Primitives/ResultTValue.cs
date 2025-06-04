namespace SharedKernal.Primitives;

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
