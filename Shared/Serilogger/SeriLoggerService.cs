using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

namespace Shared.Serilogger;

public sealed class SeriLoggerService
{
    private static ILogger? s_logger;
    public static ILogger GenerateLogger(string key)
    {
        if (s_logger is not null)
            return s_logger;

        var levelSwitch = new LoggingLevelSwitch();
        var environment = "";
#if !RELEASE
        environment = "DEBUG";
#else
        environment = "RELEASE";
#endif
        var config = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.ControlledBy(levelSwitch)
            .WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate: "[{Timestamp:HH:mm} {Level:u3}]: {Message:lj}{NewLine}{Exception}")
            .WriteTo.Seq("http://localhost:5341", apiKey: key, controlLevelSwitch: levelSwitch);

        s_logger = Log.Logger = config.CreateLogger().ForContext("Environment", environment);
        return s_logger;
    }
}
