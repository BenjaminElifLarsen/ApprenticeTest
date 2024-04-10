using Shared.DDD;

namespace CateringDataProcessingPlatform.DL.Models;

public sealed class Order : IAggregateRoot
{
    private Guid _id;
    private OrderTime _time;
    private ReferenceId _customer;
    private ReferenceId _menu;

    public Guid Id { get => _id; private set => _id = value; }
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
    }
}
