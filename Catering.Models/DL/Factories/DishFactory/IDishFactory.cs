using Catering.Shared.DL.Communication.Models;
using Catering.Shared.DL.Models;
using Shared.Patterns.ResultPattern;

namespace Catering.Shared.DL.Factories.DishFactory;

public interface IDishFactory
{
    public Result<Dish> Build(DishCreationRequest data, DishValidationData validationData);
}
