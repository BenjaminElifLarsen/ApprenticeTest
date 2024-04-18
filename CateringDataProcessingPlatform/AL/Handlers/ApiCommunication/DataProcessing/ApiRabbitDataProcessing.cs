using Catering.Shared.DL.Factories;
using CateringDataProcessingPlatform.IPL;
using CateringDataProcessingPlatform.IPL.Appsettings;
using Serilog;
using Shared.Service;

namespace CateringDataProcessingPlatform.AL.Handlers.ApiCommunication.DataProcessing;

internal partial class ApiRabbitDataProcessing : BaseService
{
    private IConfigurationManager _configurationManager;
    private IContextFactory _contextFactory;
    private IFactoryCollection _factories;

    public ApiRabbitDataProcessing(IConfigurationManager configurationManager, IContextFactory contextFactory, IFactoryCollection factoryCollection, ILogger logger) : base(logger)
    {
        _configurationManager = configurationManager;
        _contextFactory = contextFactory;
        _factories = factoryCollection;
    }
}
