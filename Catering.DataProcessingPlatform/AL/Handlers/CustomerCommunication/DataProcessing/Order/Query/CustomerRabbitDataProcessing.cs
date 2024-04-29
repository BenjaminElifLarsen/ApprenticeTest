using Catering.Shared.IPL.UnitOfWork;
using Catering.DataProcessingPlatform.AL.Handlers.CustomerCommunication.CQRS.Queries;
using Shared.Communication.Models.Menu;
using Shared.Communication.Models.Order;

namespace Catering.DataProcessingPlatform.AL.Handlers.Communication.CustomerCommunication.DataProcessing;

internal sealed partial class CustomerRabbitDataProcessing
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
            menues.Add(new GetOrdersMenuQueryResponse(order.Id, menu.Name, order.Time, menu.Id));
        }
        _logger.Debug("{Identifier}: Processed request {@GetOrders}", _identifier, request);
        return new GetOrdersQueryResponse() { Orders = menues };
    }
}
