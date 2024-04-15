using Shared.DDD;

namespace Catering.Shared.DL.Models;

public sealed class MenuOrder
{
    private ReferenceId _order;
    private DateTime _time;

    public ReferenceId Order { get => _order; private set => _order = value; }
    public DateTime Time { get => _time; private set => _time = value; }

    private MenuOrder()
    {
        
    }

    internal MenuOrder(Guid id, DateTime time)
    {
        _time = time;
        _order = new(id);
    }
}
