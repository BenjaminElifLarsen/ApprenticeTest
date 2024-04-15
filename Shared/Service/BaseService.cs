using Serilog;

namespace Shared.Service;

public abstract class BaseService
{
    protected string _identifier { get; private set; }
    protected readonly ILogger _logger;

    protected BaseService(ILogger logger)
    {
        _identifier = GetType().Name;
        _logger = logger;
    }
}
