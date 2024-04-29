using Catering.Shared.DL.Communication.Models;
using Catering.Shared.DL.Models;
using Shared.Patterns.ResultPattern;

namespace Catering.Shared.DL.Factories.MenuFactory;

public interface IMenuFactory
{
    public Result<Menu> Build(MenuCreationRequest request, MenuValidationData validationData);
}
