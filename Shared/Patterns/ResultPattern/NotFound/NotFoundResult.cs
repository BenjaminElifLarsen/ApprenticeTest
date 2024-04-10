namespace Shared.Patterns.ResultPattern.NotFound;

public sealed class NotFoundResult<T>(BinaryFlag error) : Result<T>
{
    private readonly T _data = default!;
    private readonly BinaryFlag _error = error;
    private readonly ResultType _resultType = ResultType.NotFound;

    public override T Data => _data;
    public override BinaryFlag Errors => _error;
    public override ResultType ResultType => _resultType;
}

public sealed class NotFoundResult(BinaryFlag errors) : Result
{
    private readonly BinaryFlag _error = errors;
    private readonly ResultType _resultType = ResultType.NotFound;

    public override BinaryFlag Errors => _error;
    public override ResultType ResultType => _resultType;
}
