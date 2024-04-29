using Catering.Shared.DL.Factories;
using Catering.DataProcessingPlatform.IPL;
using Catering.DataProcessingPlatform.IPL.Appsettings;
using Serilog;
using Shared.Service;

namespace Catering.DataProcessingPlatform.AL.Handlers.Communication.CustomerCommunication.DataProcessing;

internal sealed partial class CustomerRabbitDataProcessing : BaseService 
{
    private IConfigurationManager _configurationManager;
    private IContextFactory _contextFactory;
    private IFactoryCollection _factories;

    public CustomerRabbitDataProcessing(IConfigurationManager configurationManager, IContextFactory contextFactory, IFactoryCollection factoryCollection, ILogger logger) : base(logger)
    {
        _configurationManager = configurationManager;
        _contextFactory = contextFactory;
        _factories = factoryCollection;
    }
}
