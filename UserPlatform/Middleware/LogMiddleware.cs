using Microsoft.AspNetCore.Mvc.Controllers;
using Shared.Helpers.Time;
using ILogger = Serilog.ILogger;

namespace UserPlatform.Middleware;

public class LogMiddleware
{
    private readonly RequestDelegate _next;

    public LogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogger logger, ITime time)
    {
        var controllerActionDescriptor = context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
        if(controllerActionDescriptor is not null) // Always entered directly if using Swagger
        {
            var controllerName = controllerActionDescriptor.ControllerTypeInfo.Name;
            var actionName = controllerActionDescriptor.ActionName;
            var method = context.Request.Method;
            var protocol = context.Request.IsHttps ? "HTTPS" : "HTTP";
            logger.Information("{HttpProtocol}/{Controller}/{Action}:{Method} called at {Time}", protocol, controllerName, actionName, method, time.GetCurrentTimeUtc());
        }
        else // Frontend will hit this first and then the one above.
        {
            var path = context.Request.Path.Value!.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var method = context.Request.Method;
            var protocol = context.Request.IsHttps ? "HTTPS" : "HTTP";
            if (path.Length == 2)
                logger.Information("{HttpProtocol}/{Controller}/:{Method} called at {Time}", protocol, path[1] + "Controller", method, time.GetCurrentTimeUtc());
            else if (path.Length == 3) 
                logger.Information("{HttpProtocol}/{Controller}/{Action}:{Method} called at {Time}", protocol, path[1] + "Controller", path[2], method, time.GetCurrentTimeUtc());
            else
                logger.Information("{HttpProtocol}/{URL}:{Method} called at {Time}", protocol, path[0] + "Controller", method, time.GetCurrentTimeUtc());
        }

        await _next(context);
    }
}
