using Xunit.Abstractions;

namespace UserPlatform.Test.Helpers;

internal static class LogHelper
{
    public static string[] GetLogs(ITestOutputHelper output)
    {
        return (string[])((dynamic)output).Output.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
    }
}
