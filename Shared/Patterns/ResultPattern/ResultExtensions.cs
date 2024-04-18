namespace Shared.Patterns.ResultPattern;

public static class ResultExtensions
{
    public static Result ToGeneric<T>(this Result<T> result)
    {
        return result.ResultType switch
        {
            ResultType.Success => new SuccessNoDataResult(),
            ResultType.SuccessNoData => new SuccessNoDataResult(),
            ResultType.BadRequest => new BadRequestResult(result.Errors),
            ResultType.NotFound => new NotFoundResult(result.Errors),
            ResultType.Unhandled => new UnhandledResult(result.Errors),
            _ => throw new ArgumentException("Missing mapping")
        };
    }
}
