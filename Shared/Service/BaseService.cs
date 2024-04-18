using Serilog;

namespace Shared.Service;

public abstract class BaseService
{
    protected readonly string _identifier;
    protected readonly ILogger _logger;

    protected BaseService(ILogger logger)
    {
        _identifier = GetType().Name;
        _logger = logger;
    }
}
