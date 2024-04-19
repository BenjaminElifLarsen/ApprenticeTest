using Catering.API.Communication.Contract;
using Catering.API.Services.Contracts;
using Shared.Service;
using ILogger = Serilog.ILogger;

namespace Catering.API.Services.OrderService;

public sealed partial class OrderService : BaseService, IOrderService
{
    private readonly ICommunication _communication;

    public OrderService(ICommunication communication, ILogger logger) : base(logger)
    {
        _communication = communication;
    }
}
