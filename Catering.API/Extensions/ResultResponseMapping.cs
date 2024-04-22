using Microsoft.AspNetCore.Mvc;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Extensions;

public static class ResultResponseMapping
{
    public static ActionResult FromResult<T>(this ControllerBase controller, Result<T> result)
    {
        return result.ResultType switch
        {
            ResultType.Success => controller.Ok(result.Data),
            ResultType.SuccessNoData => controller.NoContent(),
            ResultType.BadRequest => controller.BadRequest(new { Errors = (long)result.Errors }),
            ResultType.InvalidAuthetication => controller.Unauthorized(),
            ResultType.NotFound => controller.NoContent(), // Some say go with 404, some with 204. This time, trying out 204
            ResultType.Unhandled => controller.Problem(statusCode: 500, detail: result.Errors.ToString()),
            _ => throw new Exception("Internal server problem"),
        };
    }
}
