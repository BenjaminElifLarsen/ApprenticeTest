using Catering.Shared.DL.Models;
using Shared.Communication.Models.Order;
using Shared.Helpers.Time;
using Shared.Patterns.ResultPattern;

namespace Catering.Shared.DL.Factories.OrderFactory;

public sealed class OrderFactory : IOrderFactory
{
    private readonly ITime _time;
    public OrderFactory(ITime time)
    {
        _time = time;        
    }

    public Result<Order> Build(OrderPlaceCommand request, OrderValidationData validationData)
    {
        BinaryFlag flag = new();
        if (request is null)
        {
            flag += OrderFactoryErrors.RequestIsNull;
            return new BadRequestResult<Order>(flag);
        }
        if (!validationData.Customers.Any(x => x.Id == request.UserId))
            flag += OrderFactoryErrors.CustomerInvalid;
        if (!validationData.Menus.Any(x => x.Id == request.MenuId))
            flag += OrderFactoryErrors.MenuInvalid;
        if (request.OrderedTo.Date <= _time.GetCurrentTimeUtc().AddDays(1).Date)
            flag += OrderFactoryErrors.TimeInvalid;
        if (!flag)
            return new BadRequestResult<Order>(flag);
        OrderTime time = new(request.OrderedTo);
        Order order = new(time, request.UserId, request.MenuId);
        return new SuccessResult<Order>(order);
    }
}
