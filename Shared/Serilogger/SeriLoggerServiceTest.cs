using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

namespace Shared.Serilogger;

public static class SeriLoggerServiceTest
{

    public static ILogger GenerateLogger()
    {
        var levelSwitch = new LoggingLevelSwitch();
        var environment = "DEBUG";
        levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Debug;
        var config = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.ControlledBy(levelSwitch)
            .WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate: "[{Timestamp:HH:mm} {Level:u3}]: {Message:lj}{NewLine}{Exception}");

        return config.CreateLogger().ForContext("Environment", environment);
    } 
}
