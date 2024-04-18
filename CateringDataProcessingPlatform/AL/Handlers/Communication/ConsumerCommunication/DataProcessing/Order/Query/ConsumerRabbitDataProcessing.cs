using Catering.Shared.IPL.UnitOfWork;
using CateringDataProcessingPlatform.AL.Handlers.Communication.ConsumerCommunication.CQRS.Queries;
using Shared.Communication.Models.Menu;
using Shared.Communication.Models.Order;

namespace CateringDataProcessingPlatform.AL.Handlers.Communication.ConsumerCommunication.DataProcessing;

internal sealed partial class ConsumerRabbitDataProcessing
{

    internal IEnumerable<MenuListQueryResponse> Process(MenuListCommand request)
    {
        _logger.Debug("{Identifier}: Processing request {@MenuList}", _identifier, request);
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_configurationManager.GetDatabaseString());
        var data = unitOfWork.MenuRepository.AllAsync(new MenuListQuery()).Result;
        _logger.Debug("{Identifier}: Processed request {@MenuList}", _identifier, request);
        return data;
    }

    internal GetOrdersQueryResponse Process(GetOrdersCommand request)
    {
        _logger.Debug("{Identifier}: Processing request {@GetOrders}", _identifier, request);
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_configurationManager.GetDatabaseString());
        var orders = unitOfWork.OrderRepository.AllByPredicate(new OrderForCustomerQuery(), x => x.Customer.Id == request.UserId).Result;
        List<GetOrdersMenuQueryResponse> menues = [];
        foreach (var order in orders)
        {
            var menu = unitOfWork.MenuRepository.GetSingleAsync(x => x.Orders.Any(xx => xx.Order.Id == order.Id)).Result;
            menues.Add(new GetOrdersMenuQueryResponse() { OrderId = order.Id, Time = order.Time, Name = menu.Name, MenuId = menu.Id });
        }
        _logger.Debug("{Identifier}: Processed request {@GetOrders}", _identifier, request);
        return new GetOrdersQueryResponse() { Orders = menues };
    }
}
