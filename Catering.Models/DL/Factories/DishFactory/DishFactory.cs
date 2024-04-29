using Catering.Shared.DL.Communication.Models;
using Catering.Shared.DL.Models;
using Shared.Patterns.ResultPattern;

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
        if (string.IsNullOrWhiteSpace(data.Name))
        {
            flag += DishCreationErrors.NameIsInvalid;
            return new BadRequestResult<Dish>(flag);
        }
        if(validationData.NamesInUse.Any(x => string.Equals(x.Name, data.Name)))
        {
            flag += DishCreationErrors.NameInUse;
            return new BadRequestResult<Dish>(flag);
        }
        Dish dish = new(data.Name);
        return new SuccessResult<Dish>(dish);
    }
}
