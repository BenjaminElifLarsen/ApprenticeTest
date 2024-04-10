using Catering.Shared.IPL.Repositories.Interfaces;
using Shared.Patterns.RepositoryPattern;

namespace Catering.Shared.IPL.UnitOfWork;

public interface IUnitOfWork : IBaseUnitOfWork
{
    public ICustomerRepository CustomerRepository { get; }
    public IDishRepository DishRepository { get; }
    public IMenuRepository MenuRepository { get; }
    public IOrderRepository OrderRepository { get; }
}
