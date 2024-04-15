using Serilog;
using Shared.Service;
using UserPlatform.Communication.Contracts;
using UserPlatform.Services.Contracts;
using UserPlatform.Shared.IPL.UnitOfWork;
using ILogger = Serilog.ILogger;

namespace UserPlatform.Services.OrderService;

internal sealed partial class OrderService : BaseService, IOrderService
{
    private readonly ICommunication _communication;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(ICommunication communication, IUnitOfWork unitOfWork, ILogger logger) : base(logger)
    {
        _communication = communication;        
        _unitOfWork = unitOfWork;
    }
}
