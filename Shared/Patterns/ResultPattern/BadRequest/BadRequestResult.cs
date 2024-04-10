namespace Shared.Patterns.ResultPattern.BadRequest;

public sealed class BadRequestResult<T>(BinaryFlag error) : Result<T>
{
    private readonly T _data = default!;
    private readonly BinaryFlag _error = error;
    private readonly ResultType _resultType = ResultType.BadRequest;

    public override T Data => _data;
    public override BinaryFlag Errors => _error;
    public override ResultType ResultType => _resultType;
}

public sealed class BadRequestResult(BinaryFlag errors) : Result
{
    private readonly BinaryFlag _errors = errors;
    private readonly ResultType _resultType = ResultType.BadRequest;

    public override BinaryFlag Errors => _errors;
    public override ResultType ResultType => _resultType;
}
