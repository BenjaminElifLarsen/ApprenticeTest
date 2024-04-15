using UserPlatform.Communication.Contracts;
using UserPlatform.Services.Contracts;
using UserPlatform.Shared.IPL.UnitOfWork;

namespace UserPlatform.Services.OrderService;

internal sealed partial class OrderService : IOrderService
{
    private readonly ICommunication _communication;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(ICommunication communication, IUnitOfWork unitOfWork)
    {
        _communication = communication;        
        _unitOfWork = unitOfWork;
    }
}
