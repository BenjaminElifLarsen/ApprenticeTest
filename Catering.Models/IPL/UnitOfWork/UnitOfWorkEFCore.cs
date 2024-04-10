using Catering.Shared.IPL.Context;
using Catering.Shared.IPL.Repositories;
using Catering.Shared.IPL.Repositories.Interfaces;
using CateringDataProcessingPlatform.DL.Models;
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
        
    }

    public ICustomerRepository CustomerRepository => throw new NotImplementedException();

    public IDishRepository DishRepository => _dishRepository;

    public IMenuRepository MenuRepository => throw new NotImplementedException();

    public IOrderRepository OrderRepository => throw new NotImplementedException();

    public void Commit()
    {
        _context.SaveChanges();
    }
}
