using Catering.Shared.DL.Models.Enums;
using Catering.Shared.Helpers;
using Catering.Shared.IPL.UnitOfWork;
using Shared.Communication.Models.Order;
using Shared.Patterns.ResultPattern;

namespace CateringDataProcessingPlatform.AL.Handlers.ApiCommunication.DataProcessing;

internal sealed partial class ApiRabbitDataProcessing
{

    internal Result Process(SetOrderStatusCommand command)
    {
        _logger.Debug("{Identifier}: Processing request {@OrderStatus}", _identifier, command);
        var validValue = EnumHelper.IsEnumValueValid((OrderState)command.OrderStatus);
        if (!validValue)
        {
            return new BadRequestResult(new(0b1));
        }
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_configurationManager.GetDatabaseString());
        var order = unitOfWork.OrderRepository.GetSingleAsync(x => x.Id == command.OrderId).Result;
        if (order is null)
        {
            return new NotFoundResult(new BinaryFlag(0b10));
        }
        bool statusChanged = false;
        switch ((OrderState)command.OrderStatus)
        {
            case OrderState.Preparing:
                statusChanged = order.Preparing();
                break;

            case OrderState.ReadyToDeliver:
                statusChanged = order.DeliverReady();
                break;

            case OrderState.Delivered:
                statusChanged = order.Delivered();
                break;

            case OrderState.FailedToBeDelivered:
                statusChanged = order.FailedToDeliver();
                break;

            default:
                break;
        }

        if (statusChanged)
        {
            unitOfWork.OrderRepository.Update(order);
            unitOfWork.Commit();
        }

        _logger.Debug("{Identifier}: Processed request {@OrderStatus}", _identifier, command);
        return statusChanged ? new SuccessNoDataResult() : new BadRequestResult(new(0b100));
    }
}
