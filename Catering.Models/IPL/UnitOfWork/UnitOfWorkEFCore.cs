using Catering.Shared.IPL.Context;
using Catering.Shared.IPL.Repositories;
using Catering.Shared.IPL.Repositories.Interfaces;
using Catering.Shared.DL.Models;
using Shared.Patterns.RepositoryPattern;

namespace Catering.Shared.IPL.UnitOfWork;

public class UnitOfWorkEFCore : IUnitOfWork
{
    private readonly CateringContext _context;
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IDishRepository _dishRepository;
    private readonly IMenuRepository _menuRepository;

    public UnitOfWorkEFCore(CateringContext context)
    {
        _context = context;
        _dishRepository = new DishRepository(new EntityFrameworkCoreRepository<Dish, CateringContext>(_context));
        _orderRepository = new OrderRepository(new EntityFrameworkCoreRepository<Order, CateringContext>(context));
        _menuRepository = new MenuRepository(new EntityFrameworkCoreRepository<Menu,  CateringContext>(context));
        _customerRepository = new CustomerRepository(new EntityFrameworkCoreRepository<Customer, CateringContext>(context));
    }

    public ICustomerRepository CustomerRepository => _customerRepository;

    public IDishRepository DishRepository => _dishRepository;

    public IMenuRepository MenuRepository => _menuRepository;

    public IOrderRepository OrderRepository => _orderRepository;

    public void Commit()
    {
        _context.SaveChanges();
    }
}
