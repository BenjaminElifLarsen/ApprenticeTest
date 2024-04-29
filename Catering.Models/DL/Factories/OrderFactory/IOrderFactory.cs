using Catering.Shared.DL.Models;
using Shared.Communication.Models.Order;
using Shared.Patterns.ResultPattern;

namespace Catering.Shared.DL.Factories.OrderFactory;

public interface IOrderFactory
{

    public Result<Order> Build(OrderPlaceCommand request, OrderValidationData validationData);
}
