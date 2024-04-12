using UserPlatform.Communication.Contracts;
using UserPlatform.Services.Contracts;

namespace UserPlatform.Services.OrderService;

internal sealed partial class OrderService : IOrderService
{
    private readonly ICommunication _communication;
    public OrderService(ICommunication communication)
    {
        _communication = communication;        
    }
}
