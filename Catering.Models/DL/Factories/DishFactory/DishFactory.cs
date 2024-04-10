using Catering.Shared.DL.Communication.Models;
using CateringDataProcessingPlatform.DL.Models;
using Shared.Patterns.ResultPattern;
using Shared.Patterns.ResultPattern.BadRequest;

namespace Catering.Shared.DL.Factories.DishFactory;

public class DishFactory : IDishFactory
{   
    public Result<Dish> Build(DishCreationRequest data, DishValidationData validationData)
    {
        BinaryFlag flag = new();
        if (data is null)
        {
            flag += DishCreationErrors.RequestIsNull;
            return new BadRequestResult<Dish>(flag);
        }
        if (string.IsNullOrEmpty(data.Name))
        {
            flag += DishCreationErrors.NameIsInvalid;
            return new BadRequestResult<Dish>(flag);
        }
        if(validationData.NamesInUse.Any(x => string.Equals(x, data.Name)))
        {
            flag += DishCreationErrors.NameInUse;
            return new BadRequestResult<Dish>(flag);
        }
        Dish dish = new(data.Name);
        return new SuccessResult<Dish>(dish);
    }
}
