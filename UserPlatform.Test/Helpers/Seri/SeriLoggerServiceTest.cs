using Serilog;
using Serilog.Core;
using Xunit.Abstractions;

namespace UserPlatform.Test.Helpers.Seri;

public static class SeriLoggerServiceTest // TODO: if implementing for other test project, create a shared test project version, together with the rest of the classes in Helpers
{
    public static ILogger GenerateLogger(ITestOutputHelper output, string context)
    {
        var levelSwitch = new LoggingLevelSwitch();
        var environment = "Test";
        levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Verbose;
        var config = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.ControlledBy(levelSwitch)
            .WriteTo.TestOutput(output, outputTemplate: "[{Timestamp:HH:mm} {Level:u3}]: {Message:lj}{NewLine}{Exception}");
        return config.CreateLogger().ForContext("Environment", environment).ForContext("Test Class", context);
    }
}
