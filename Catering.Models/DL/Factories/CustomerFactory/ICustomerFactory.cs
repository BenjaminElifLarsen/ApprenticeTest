using CateringDataProcessingPlatform.DL.Models;
using Shared.Communication.Models;
using Shared.Patterns.ResultPattern;

namespace Catering.Shared.DL.Factories.CustomerFactory;

public interface ICustomerFactory
{
    public Result<Customer> Build(UserCreationCommand command, CustomerValidationData validationData);
}
