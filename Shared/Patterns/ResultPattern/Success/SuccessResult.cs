namespace Shared.Patterns.ResultPattern;

public sealed class SuccessResult<T>(T data) : Result<T>
{
    private readonly T _data = data;
    private readonly BinaryFlag _error = new();
    private readonly ResultType _resultType = ResultType.Success;

    public override T Data => _data;
    public override BinaryFlag Errors => _error;
    public override ResultType ResultType => _resultType;
}
