using Catering.Shared.DL.Communication.Models;
using CateringDataProcessingPlatform.DL.Models;
using Shared.Patterns.ResultPattern;

namespace Catering.Shared.DL.Factories.DishFactory;

internal interface IDishFactory
{
    public Result<Dish> Build(DishCreationRequest data, DishValidationData validationData);
}
