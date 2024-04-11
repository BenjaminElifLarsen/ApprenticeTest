using Shared.Patterns.ResultPattern;
using UserPlatform.Shared.Communication.Models;
using UserPlatform.Shared.DL.Models;

namespace UserPlatform.Shared.DL.Factories;

public interface IUserFactory
{
    public Result<User> Build(UserCreationRequest request, UserValidationData validationData);
}
