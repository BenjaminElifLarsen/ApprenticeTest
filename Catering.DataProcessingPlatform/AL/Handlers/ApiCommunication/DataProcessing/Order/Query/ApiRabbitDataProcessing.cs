﻿using Catering.Shared.DL.Models.Enums;
using Catering.Shared.IPL.UnitOfWork;
using Catering.DataProcessingPlatform.AL.Handlers.ApiCommunication.CQRS.Queries;
using Shared.Communication.Models.Order;
using Shared.Patterns.ResultPattern;
using Catering.Shared.DL.Models;

namespace Catering.DataProcessingPlatform.AL.Handlers.ApiCommunication.DataProcessing;

internal sealed partial class ApiRabbitDataProcessing
{
    internal Result<GetOrdersFututureQueryResponse> Process(GetFutureOrdersCommand command)
    {
        _logger.Debug("{Identifier}: Processing request {@OrderFuture}", _identifier, command);
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_configurationManager.GetDatabaseString());
        var afterTime = _time.GetCurrentTimeUtc();
        var futureOrders = unitOfWork.OrderRepository.AllByPredicate(new OrderDetailsQuery(), x => x.Time.Time > afterTime && x.Status is OrderState.Received or OrderState.Preparing).Result;
        List<GetOrdersFutureDetailsQueryResponse> details = [];
        List<Menu> alreadyFoundMenues = [];
        List<Customer> alreadyFoundCustomers = [];
        foreach(var order in futureOrders)
        {
            var menu = alreadyFoundMenues.FirstOrDefault(x => x.Id == order.MenuId);
            menu ??= unitOfWork.MenuRepository.GetSingleAsync(x => x.Id == order.MenuId).Result;
            var customer = alreadyFoundCustomers.FirstOrDefault(x => x.Id == order.CustomerId);
            customer ??= unitOfWork.CustomerRepository.GetSingleAsync(x => x.Id == order.CustomerId).Result;
            details.Add(new(order.OrderId, order.OrderedTo, order.MenuId, menu.Name, order.CustomerId, customer.CustomerName));
        }
        GetOrdersFututureQueryResponse gofqr = new(details);
        _logger.Debug("{Identifier}: Processed request {@OrderFuture}", _identifier, command);
        return new SuccessResult<GetOrdersFututureQueryResponse>(gofqr);
    }

    internal Result<GetOrderOverviewQueryResponse> Process(GetOrderOverviewQueryCommand command)
    {
        _logger.Debug("{Identifier}: Processing request {@OrderOverview}", _identifier, command);
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_configurationManager.GetDatabaseString());
        var orders = unitOfWork.OrderRepository.AllAsync(new OrderStatusQuery()).Result;
        GetOrderOverviewQueryResponse gooqr = new(orders);
        _logger.Debug("{Identifier}: Processed request {@OrderOverview}", _identifier, command);
        return new SuccessResult<GetOrderOverviewQueryResponse>(gooqr);
    }
}
