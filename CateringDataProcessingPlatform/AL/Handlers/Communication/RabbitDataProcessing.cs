using Catering.Shared.DL.Factories;
using Catering.Shared.DL.Factories.CustomerFactory;
using Catering.Shared.DL.Factories.OrderFactory;
using Catering.Shared.IPL.UnitOfWork;
using CateringDataProcessingPlatform.AL.Handlers.Communication.CQRS.Queries;
using CateringDataProcessingPlatform.IPL;
using CateringDataProcessingPlatform.IPL.Appsettings;
using Serilog;
using Shared.Communication.Models;
using Shared.Service;

namespace CateringDataProcessingPlatform.AL.Handlers.Communication;

internal sealed class RabbitDataProcessing : BaseService
{
    private IConfigurationManager _configurationManager;
    private IContextFactory _contextFactory;
    private ILogger _logger;
    private IFactoryCollection _factories;

    public RabbitDataProcessing(IConfigurationManager configurationManager, IContextFactory contextFactory, IFactoryCollection factoryCollection, ILogger logger)
    {
        _configurationManager = configurationManager;
        _contextFactory = contextFactory;        
        _logger = logger;
        _factories = factoryCollection;
    }

    internal void Process(OrderPlaceCommand request)
    {
        _logger.Debug("{Identifier}: Processing request {OrderPlace}", _identifier, request);
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_contextFactory.CreateDbContext([_configurationManager.GetDatabaseString()])); // TODO: improve this...
        var existingCustomers = unitOfWork.CustomerRepository.AllAsync(new OrderCustomerDataQuery()).Result;
        var existingMenues = unitOfWork.MenuRepository.AllAsync(new OrderMenuDataQuery()).Result;
        OrderValidationData ovd = new(existingCustomers, existingMenues);
        var factory = _factories.OrderFactory;
        var result = factory.Build(request, ovd);
        if (!result)
        {
            _logger.Warning("{Identifier}: Failed at creating order {OrderPlace} - {Errors}", _identifier, request, result);
            return;
        }
        unitOfWork.OrderRepository.Create(result.Data);
        unitOfWork.Commit();
    }

    internal IEnumerable<MenuListQueryResponse> Process(MenuListCommand request)
    {
        _logger.Debug("{Identifier}: Processing request {MenuList}", _identifier, request);
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_contextFactory.CreateDbContext([_configurationManager.GetDatabaseString()])); // TODO: improve this...
        var data = unitOfWork.MenuRepository.AllAsync(new MenuListQuery()).Result;
        return data;

    }

    internal void Process(UserCreationCommand request)
    {
        _logger.Debug("{Identifier}: Processing request {CustomerCreation}", _identifier, request);
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_contextFactory.CreateDbContext([_configurationManager.GetDatabaseString()])); // TODO: improve this...
        var existingCustomers = unitOfWork.CustomerRepository.AllAsync(new CustomerDataQuery()).Result;
        CustomerValidationData cvd = new(existingCustomers);
        var factory = _factories.CustomerFactory;
        var result = factory.Build(request, cvd);
        if(!result)
        {
            _logger.Warning("{Identifier}: Failed at creating customer {CustomerCreation} - {Errors}", _identifier, request, result);
            return;
        }
        unitOfWork.CustomerRepository.Create(result.Data);
        unitOfWork.Commit();
    }
}
