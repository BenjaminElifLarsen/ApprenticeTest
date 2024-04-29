using Catering.Shared.DL.Models;
using Shared.Communication.Models.User;
using Shared.Patterns.ResultPattern;

namespace Catering.Shared.DL.Factories.CustomerFactory;

public interface ICustomerFactory
{
    public Result<Customer> Build(UserCreationCommand command, CustomerValidationData validationData);
}
