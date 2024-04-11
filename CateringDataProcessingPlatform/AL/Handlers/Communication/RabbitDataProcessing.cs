using CateringDataProcessingPlatform.IPL;
using CateringDataProcessingPlatform.IPL.Appsettings;
using Serilog;

namespace CateringDataProcessingPlatform.AL.Handlers.Communication;

internal sealed class RabbitDataProcessing
{
    private IConfigurationManager _configurationManager;
    private IContextFactory _contextFactory;
    private ILogger _logger;

    public RabbitDataProcessing(IConfigurationManager configurationManager, IContextFactory contextFactory, ILogger logger)
    {
        _configurationManager = configurationManager;
        _contextFactory = contextFactory;        
        _logger = logger;
    }
    
}
