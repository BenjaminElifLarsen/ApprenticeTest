namespace Shared.Patterns.ResultPattern;

public sealed class UnhandledResult<T>(BinaryFlag error) : Result<T>
{
    private readonly T _data = default!;
    private readonly BinaryFlag _error = error;
    private readonly ResultType _resultType = ResultType.Unhandled;

    public override T Data => _data;
    public override BinaryFlag Errors => _error;
    public override ResultType ResultType => _resultType;
}

public sealed class UnhandledResult(BinaryFlag error) : Result
{
    private readonly BinaryFlag _error = error;
    private readonly ResultType _resultType = ResultType.Unhandled;

    public override BinaryFlag Errors => _error;
    public override ResultType ResultType => _resultType;
}
