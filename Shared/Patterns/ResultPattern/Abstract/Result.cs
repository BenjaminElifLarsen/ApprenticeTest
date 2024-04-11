namespace Shared.Patterns.ResultPattern;

public abstract class Result<T>
{
    public abstract T Data { get; }
    public abstract BinaryFlag Errors { get; }
    public abstract ResultType ResultType { get; }

    public static implicit operator bool(Result<T> result) => result is not null
        && result.ResultType is ResultType.Success;

    public override string ToString()
    {
        return Errors.ToString();
    }
}

public abstract class Result : Result<object>
{
    private readonly object _data = default!;
    public override object Data => _data;    

    public static implicit operator bool(Result result) => result is not null
        && result.ResultType is ResultType.SuccessNoData;
}

