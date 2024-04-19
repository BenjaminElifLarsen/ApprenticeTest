using Catering.API.Communication.Contract;
using Catering.API.Services.Contracts;
using Shared.Service;
using ILogger = Serilog.ILogger;

namespace Catering.API.Services.DishService;

public sealed partial class DishService : BaseService, IDishService
{
    private readonly ICommunication _communication;

    public DishService(ICommunication communication, ILogger logger) : base(logger)
    {
        _communication = communication;
    }
}
