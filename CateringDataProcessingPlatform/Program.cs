using Catering.Shared.IPL.UnitOfWork;
using CateringDataProcessingPlatform.AL.Handlers.Communication;
using CateringDataProcessingPlatform.AL.Services.Contracts;
using CateringDataProcessingPlatform.AL.Services.SeederService;
using CateringDataProcessingPlatform.IPL;
using CateringDataProcessingPlatform.IPL.Appsettings;
using Serilog;
using Shared.Serilogger;

IConfigurationManager manager = new ConfigurationManager();
var cs = manager.GetDatabaseString();
ContextEFFactory cf = new();
var cc = cf.CreateDbContext([cs]);
IUnitOfWork uow = new UnitOfWorkEFCore(cc);
ILogger logger = SeriLoggerService.GenerateLogger(manager.GetLogKey());
ISeederService seederService = new SeederService(uow, logger);
seederService.Seed();
seederService = null!;
var rabbit = manager.GetRabbit();
RabbitCommunication communication = new(manager, cf, rabbit, logger);
communication.Initialise();
while (true)
{
    Thread.Sleep(TimeSpan.FromMinutes(10));
}



