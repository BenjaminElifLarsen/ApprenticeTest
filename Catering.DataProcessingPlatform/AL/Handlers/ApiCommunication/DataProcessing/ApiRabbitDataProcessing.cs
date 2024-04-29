using Catering.Shared.DL.Factories;
using Catering.DataProcessingPlatform.IPL;
using Catering.DataProcessingPlatform.IPL.Appsettings;
using Serilog;
using Shared.Helpers.Time;
using Shared.Service;

namespace Catering.DataProcessingPlatform.AL.Handlers.ApiCommunication.DataProcessing;

internal partial class ApiRabbitDataProcessing : BaseService
{
    private readonly ITime _time;
    private readonly IConfigurationManager _configurationManager;
    private readonly IContextFactory _contextFactory;
    private readonly IFactoryCollection _factories;

    public ApiRabbitDataProcessing(IConfigurationManager configurationManager, IContextFactory contextFactory, IFactoryCollection factoryCollection, ITime time, ILogger logger) : base(logger)
    {
        _configurationManager = configurationManager;
        _contextFactory = contextFactory;
        _factories = factoryCollection;
        _time = time;
    }
}
