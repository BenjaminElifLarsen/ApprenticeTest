using Catering.API.Communication.Contract;
using Catering.API.Services.Contracts;
using Shared.Service;
using ILogger = Serilog.ILogger;

namespace Catering.API.Services.MenuService;

public sealed partial class MenuService : BaseService, IMenuService
{
    private readonly ICommunication _communication;

    public MenuService(ICommunication communication, ILogger logger) : base(logger)
    {
        _communication = communication;
    }
}
