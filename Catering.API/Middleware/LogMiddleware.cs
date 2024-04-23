using Microsoft.AspNetCore.Mvc.Controllers;
using Serilog;
using Shared.Helpers.Time;
using ILogger = Serilog.ILogger;

namespace Catering.API.Middleware;

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
        var controllerName = controllerActionDescriptor.ControllerTypeInfo.Name;
        var actionName = controllerActionDescriptor.ActionName;
        var method = context.Request.Method;
        var protocol = context.Request.IsHttps ? "HTTPS" : "HTTP";
        logger.Information("{HttpProtocol}.{Controller}.{Action}:{Method} called at {Time}", protocol, controllerName, actionName, method, time.GetCurrentTimeUtc());

        await _next(context);
    }
}
