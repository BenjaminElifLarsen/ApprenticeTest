namespace Shared.Patterns.ResultPattern;

public sealed class SuccessNoDataResult : Result
{
    private readonly BinaryFlag _error;
    private readonly ResultType _resultType;

    public override BinaryFlag Errors => _error;
    public override ResultType ResultType => _resultType;

    public SuccessNoDataResult()
    {
        _error = new BinaryFlag();
        _resultType = ResultType.SuccessNoData;        
    }
}
