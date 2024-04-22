using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace UserPlatform.Helpers
{
    public class BaseApiController : ControllerBase
    {
        protected readonly string _identifier;
        protected readonly ILogger _logger;

        protected BaseApiController(ILogger logger)
        {
            _identifier = GetType().Name;
            _logger = logger;
        }
    }
}
