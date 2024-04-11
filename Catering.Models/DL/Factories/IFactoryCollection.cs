using Catering.Shared.DL.Factories.CustomerFactory;
using Catering.Shared.DL.Factories.DishFactory;
using Catering.Shared.DL.Factories.MenuFactory;
using Catering.Shared.DL.Factories.OrderFactory;

namespace Catering.Shared.DL.Factories;

public interface IFactoryCollection
{
    public ICustomerFactory CustomerFactory { get; }
    public IDishFactory DishFactory { get; }
    public IMenuFactory MenuFactory { get; }
    public IOrderFactory OrderFactory { get; }
}
