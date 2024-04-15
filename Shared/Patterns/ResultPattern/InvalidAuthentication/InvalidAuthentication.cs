namespace Shared.Patterns.ResultPattern;

public sealed class InvalidAuthentication<T> : Result<T>
{
    private readonly T _data = default!;
    private readonly BinaryFlag _error = new();
    private readonly ResultType _resultType = ResultType.InvalidAuthetication;

    public override T Data => _data;

    public override BinaryFlag Errors => _error;

    public override ResultType ResultType => _resultType;
}
