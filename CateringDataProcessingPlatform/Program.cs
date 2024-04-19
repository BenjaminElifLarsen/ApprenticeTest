using Catering.Shared.DL.Factories;
using Catering.Shared.IPL.UnitOfWork;
using CateringDataProcessingPlatform.AL.Handlers.ApiCommunication;
using CateringDataProcessingPlatform.AL.Handlers.CustomerCommunication;
using CateringDataProcessingPlatform.AL.Services.Contracts;
using CateringDataProcessingPlatform.AL.Services.SeederService;
using CateringDataProcessingPlatform.IPL;
using CateringDataProcessingPlatform.IPL.Appsettings;
using Serilog;
using Shared.Helpers.Time;
using Shared.Serilogger;

ILogger logger = null!;
ITime time = null!;
CustomerRabbitCommunication customerCommunication = null!;
ApiRabbitCommunication apiCommunication = null!;
AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

time = new Time("Romance Standard Time");
IConfigurationManager manager = new ConfigurationManager();
logger = SeriLoggerService.GenerateLogger(manager.GetLogKey());
logger.Information("{ProcessName} starting at {Time}", AppDomain.CurrentDomain.FriendlyName, time.GetCurrentTimeUtc());

var connectionString = manager.GetDatabaseString();
ContextEFFactory cf = new();
var contextFactory = cf.CreateDbContext([connectionString]);
IFactoryCollection factoryCollection = new FactoryCollection(time);
IUnitOfWork unitOfWork = new UnitOfWorkEFCore(contextFactory);
var rabbit = manager.GetRabbit();

#if !RELEASE
ISeederService seederService = new SeederService(unitOfWork, logger);
seederService.Seed();
seederService = null!;
#endif

customerCommunication = new(manager, cf, factoryCollection, rabbit, logger);
apiCommunication = new(manager, cf, factoryCollection, rabbit, time, logger);
customerCommunication.Initialise();
apiCommunication.Initialise();

logger.Information("{ProcessName} fully running at {Time}", AppDomain.CurrentDomain.FriendlyName, time.GetCurrentTimeUtc());
while (true)
{
    Thread.Sleep(TimeSpan.FromMinutes(10));
}

/// Is not an excuse to not handle exceptions. Reaching this point means the program has reached a fully unstable state.
void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
{
    if(logger is not null)
    {
        if(e.ExceptionObject is Exception ex)
            logger.Fatal(ex, "Unhandled exception encounted");
        else
            logger.Fatal("Unhandled exception encounted, object not derived from exception - {@UnhandledException}", e);
    }
    AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
    AppDomain.CurrentDomain.ProcessExit -= CurrentDomain_ProcessExit;
    customerCommunication?.Dispose();
    apiCommunication?.Dispose();
    //Could call Process.Start("CateringDataProcessingPlatform.exe") to start a new process 
    Environment.Exit(512); // Random chosen number
}


void CurrentDomain_ProcessExit(object? sender, EventArgs e)
{
    if(logger is not null)
    {
        if(time is not null)
            logger.Information("{ProcessName} shutdowned at {Time}", AppDomain.CurrentDomain.FriendlyName, time.GetCurrentTimeUtc());
        else
            logger.Information("{ProcessName} shutdowned", AppDomain.CurrentDomain.FriendlyName);
    }
    AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
    AppDomain.CurrentDomain.ProcessExit -= CurrentDomain_ProcessExit;
    customerCommunication?.Dispose();
    apiCommunication?.Dispose();
    Environment.Exit(0);
}