using Catering.Shared.DL.Factories;
using Catering.Shared.IPL.UnitOfWork;
using CateringDataProcessingPlatform.AL.Handlers.Communication;
using CateringDataProcessingPlatform.AL.Services.Contracts;
using CateringDataProcessingPlatform.AL.Services.SeederService;
using CateringDataProcessingPlatform.IPL;
using CateringDataProcessingPlatform.IPL.Appsettings;
using Serilog;
using Shared.Helpers.Time;
using Shared.Serilogger;

ITime time = new Time();
IConfigurationManager manager = new ConfigurationManager();
ILogger logger = SeriLoggerService.GenerateLogger(manager.GetLogKey());
var connectionString = manager.GetDatabaseString();
ContextEFFactory cf = new();
var contextFactory = cf.CreateDbContext([connectionString]);
IUnitOfWork unitOfWork = new UnitOfWorkEFCore(contextFactory);
#if !RELEASE
ISeederService seederService = new SeederService(unitOfWork, logger);
seederService.Seed();
seederService = null!;
#endif
var rabbit = manager.GetRabbit();

IFactoryCollection factoryCollection = new FactoryCollection(time);
RabbitCommunication communication = new(manager, cf, factoryCollection, rabbit, logger);
communication.Initialise();
while (true)
{
    Thread.Sleep(TimeSpan.FromMinutes(10));
}

// TODO: catch unhandled exceptions


