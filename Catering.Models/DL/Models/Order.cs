using Catering.Shared.DL.Models.Enums;
using Shared.DDD;

namespace CateringDataProcessingPlatform.DL.Models;

public sealed class Order : IAggregateRoot
{
    private Guid _id;
    private OrderTime _time;
    private ReferenceId _customer;
    private OrderState _status;
    private ReferenceId _menu;

    public Guid Id { get => _id; private set => _id = value; }
    public OrderState Status { get => _status; private set => _status = value; }
    public OrderTime Time { get => _time; private set => _time = value; }
    public ReferenceId Customer { get => _customer; private set => _customer = value; }
    public ReferenceId Menu { get => _menu; private set => _menu = value; }

    private Order()
    {
        
    }

    internal Order(OrderTime time, Guid customerId, Guid menuId)
    {
        _id = Guid.NewGuid();
        _time = time;
        _customer = new(customerId);
        _menu = new(menuId);
        _status = OrderState.Received;
    }

    public bool Preparing()
    {
        if (_status is OrderState.Received)
        {
            _status = OrderState.Preparing;
            return true;
        }
        return false;
    }

    public bool DeliverReady()
    {
        if(Status is OrderState.Preparing)
        {
            _status = OrderState.ReadyToDeliver;
            return true;
        }
        return false;
    }

    public bool Delivered()
    {
        if (Status is OrderState.ReadyToDeliver)
        { 
            _status = OrderState.Delivered;
            return true;
        }
        return false;
    }

    public bool FailedToDeliver()
    {
        if (Status is OrderState.ReadyToDeliver)
        {
            _status = OrderState.FailedToBeDelivered;
            return true;
        }
        return false;
    }
}
