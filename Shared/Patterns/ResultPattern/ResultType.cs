namespace Shared.Patterns.ResultPattern;

public enum ResultType
{
    Success = 1,
    SuccessNoData = 2,
    BadRequest = 3,
    NotFound = 4,
    Unhandled = 5,
    InvalidAuthetication = 6,
    Unknown = 0,
}
