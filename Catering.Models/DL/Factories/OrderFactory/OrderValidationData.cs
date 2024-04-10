using CateringDataProcessingPlatform.DL.Models;
using Shared.Patterns.CQRS.Queries;
using System.Linq.Expressions;

namespace Catering.Shared.DL.Factories.OrderFactory;

public sealed class OrderValidationData
{
    public IEnumerable<OrderCustomerData> Customers { get; private set; }
    public IEnumerable<OrderMenuData> Menus { get; private set; }

    public OrderValidationData(IEnumerable<OrderCustomerData> customers, IEnumerable<OrderMenuData> orders)
    {
        Customers = customers;
        Menus = orders;        
    }
}

public sealed class OrderCustomerData : BaseReadModel
{
    public Guid Id { get; private set; }

    public OrderCustomerData(Guid id)
    {
        Id = id;        
    }
}

public sealed class OrderMenuData : BaseReadModel
{
    public Guid Id { get; private set; }

    public OrderMenuData(Guid id)
    {
        Id = id;
    }
}

public sealed class OrderCustomerDataQuery : BaseQuery<Customer, OrderCustomerData>
{
    public override Expression<Func<Customer, OrderCustomerData>> Map()
    {
        return e => new(e.Id);
    }
}

public sealed class OrderMenuDataQuery : BaseQuery<Menu, OrderMenuData>
{
    public override Expression<Func<Menu, OrderMenuData>> Map()
    {
        return e => new(e.Id);
    }
}
