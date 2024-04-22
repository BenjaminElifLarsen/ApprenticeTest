using Catering.Shared.DL.Models.Enums;

namespace Catering.API.Models.Order.Request;

public sealed class SetOrderStatusRequest
{
    public Guid OrderId { get; set; }

    internal OrderState Status { get; private set; }

    internal void SetStatus(OrderState status) => Status = status;
}
