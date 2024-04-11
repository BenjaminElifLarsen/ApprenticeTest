using Catering.Shared.DL.Factories.CustomerFactory;
using Catering.Shared.DL.Factories.DishFactory;
using Catering.Shared.DL.Factories.MenuFactory;
using Catering.Shared.DL.Factories.OrderFactory;
using Shared.Helpers.Time;

namespace Catering.Shared.DL.Factories;

public class FactoryCollection : IFactoryCollection
{
    private readonly ICustomerFactory _customerFactory;
    private readonly IDishFactory _dishFactory;
    private readonly IMenuFactory _menuFactory;
    private readonly IOrderFactory _orderFactory;

    public FactoryCollection(ITime time)
    {
        _customerFactory = new CustomerFactory.CustomerFactory();
        _dishFactory = new DishFactory.DishFactory();
        _menuFactory = new MenuFactory.MenuFactory();
        _orderFactory = new OrderFactory.OrderFactory(time);
    }

    public ICustomerFactory CustomerFactory => _customerFactory;

    public IDishFactory DishFactory => _dishFactory;

    public IMenuFactory MenuFactory => _menuFactory;

    public IOrderFactory OrderFactory => _orderFactory;
}
