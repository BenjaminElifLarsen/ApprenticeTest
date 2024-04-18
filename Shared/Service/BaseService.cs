using Serilog;

namespace Shared.Service;

/// <summary>
/// Base service with logger and service identification
/// </summary>
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
