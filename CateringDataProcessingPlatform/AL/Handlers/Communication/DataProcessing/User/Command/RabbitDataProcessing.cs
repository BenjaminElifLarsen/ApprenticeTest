using Catering.Shared.DL.Factories.CustomerFactory;
using Catering.Shared.IPL.UnitOfWork;
using Shared.Communication.Models.User;

namespace CateringDataProcessingPlatform.AL.Handlers.Communication.DataProcessing;

internal sealed partial class RabbitDataProcessing
{
    internal void Process(UserCreationCommand request)
    {
        _logger.Debug("{Identifier}: Processing request {@CustomerCreation}", _identifier, request);
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_configurationManager.GetDatabaseString());
        var existingCustomers = unitOfWork.CustomerRepository.AllAsync(new CustomerDataQuery()).Result;
        CustomerValidationData cvd = new(existingCustomers);
        var factory = _factories.CustomerFactory;
        var result = factory.Build(request, cvd);
        if (!result)
        {
            _logger.Warning("{Identifier}: Failed at creating customer {CustomerCreation} - {Errors}", _identifier, request, result);
            return;
        }
        unitOfWork.CustomerRepository.Create(result.Data);
        unitOfWork.Commit();
        _logger.Debug("{Identifier}: Processed request {@CustomerCreation}", _identifier, request);
    }
}
