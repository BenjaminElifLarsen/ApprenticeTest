﻿using Catering.Shared.DL.Factories.OrderFactory;
using Catering.Shared.IPL.UnitOfWork;
using CateringDataProcessingPlatform.AL.Handlers.Communication.CQRS.Queries;
using Shared.Communication.Models.Menu;
using Shared.Communication.Models.Order;
using Shared.Communication.Models.User;

namespace CateringDataProcessingPlatform.AL.Handlers.Communication.DataProcessing;

internal sealed partial class RabbitDataProcessing
{
    internal void Process(OrderPlaceCommand request)
    {
        _logger.Debug("{Identifier}: Processing request {@OrderPlace}", _identifier, request);
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_configurationManager.GetDatabaseString());
        var existingCustomers = unitOfWork.CustomerRepository.AllAsync(new OrderCustomerDataQuery()).Result;
        var existingMenues = unitOfWork.MenuRepository.AllAsync(new OrderMenuDataQuery()).Result;
        OrderValidationData ovd = new(existingCustomers, existingMenues);
        var factory = _factories.OrderFactory;
        var result = factory.Build(request, ovd);
        if (!result)
        {
            _logger.Warning("{Identifier}: Failed at creating order {OrderPlace} - {Errors}", _identifier, request, result);
            return;
        }
        var menu = unitOfWork.MenuRepository.GetSingleAsync(x => x.Id == request.MenuId).Result;
        var customer = unitOfWork.CustomerRepository.GetSingleAsync(x => x.Id == request.UserId).Result;
        menu.AddOrder(result.Data.Id, result.Data.Time.Time);
        customer.AddOrder(result.Data.Id);
        unitOfWork.OrderRepository.Create(result.Data);
        unitOfWork.MenuRepository.Update(menu);
        unitOfWork.CustomerRepository.Update(customer);
        unitOfWork.Commit();
        _logger.Debug("{Identifier}: Processed request {@OrderPlace}", _identifier, request);
    }
}
