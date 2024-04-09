using Shared.DDD;

namespace CateringDataProcessingPlatform.DL.Models;

internal sealed class Order : IAggregateRoot
{
    private Guid _id;
    private OrderTime _time;
    private ReferenceId _customer;
    private ReferenceId _menu;

    public Guid Id => throw new NotImplementedException();
}
