using Microsoft.AspNetCore.Mvc;

namespace UserPlatform.Test.Helpers;

internal static class ActionHelper
{
    public static int GetStatusCode(IActionResult actionResult)
        => ((dynamic)actionResult).StatusCode;
    
    public static T GetData<T>(IActionResult actionResult)
        => ((T)((dynamic)actionResult).Value);
}
