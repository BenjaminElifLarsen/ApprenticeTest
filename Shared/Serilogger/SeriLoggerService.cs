using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

namespace Shared.Serilogger;

public static class SeriLoggerService
{
    private static readonly Dictionary<string, ILogger> s_loggers;

    static SeriLoggerService()
    {
        s_loggers = [];        
    }

    public static ILogger GenerateLogger(string key)
    {
        if(s_loggers.TryGetValue(key, out ILogger? log))
            return log;

        var levelSwitch = new LoggingLevelSwitch();
        var environment = "";
#if !RELEASE
        environment = "DEBUG";
        levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Debug;
#else
        environment = "RELEASE";
        levelSwitch.MaximumLevel = Serilog.Events.LogEventLevel.Information;
#endif
        var config = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.ControlledBy(levelSwitch)
            .WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate: "[{Timestamp:HH:mm} {Level:u3}]: {Message:lj}{NewLine}{Exception}")
            .WriteTo.Seq("http://localhost:5341", apiKey: key, controlLevelSwitch: levelSwitch); // Normally, it would write to the SEQ only in the case of no debugger is attached.

        log = config.CreateLogger().ForContext("Environment", environment);
        s_loggers.Add(key, log);
        return log;
    }
}
