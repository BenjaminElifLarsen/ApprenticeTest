using Catering.Shared.DL.Communication.Models;
using CateringDataProcessingPlatform.DL.Models;
using Shared.Patterns.ResultPattern;
using Shared.Patterns.ResultPattern.BadRequest;

namespace Catering.Shared.DL.Factories.MenuFactory;

public sealed class MenuFactory : IMenuFactory
{
    public Result<Menu> Build(MenuCreationRequest request, MenuValidationData validationData)
    {
        BinaryFlag flag = new();
        if(request is null)
        {
            flag += MenuFactoryErrors.RequestIsNull;
            return new BadRequestResult<Menu>(flag);
        }
        if(!request.Parts.Any(x => validationData.KnownDishes.Any(xx => x.Id == xx.Id)))
        {
            flag += MenuFactoryErrors.OneOrMoreDishesDoNotExist;
        }
        if (request.Parts.Any(x => x.Price <= 0))
            flag += MenuFactoryErrors.OneOrMoreDishesDoNotHaveAPrice;
        if (string.IsNullOrWhiteSpace(request.Name))
            flag += MenuFactoryErrors.NameIsNotValid;
        if (validationData.UsedNames.Any(x => string.Equals(x, request.Name)))
            flag += MenuFactoryErrors.NameInUse;
        if (request.Parts.Any(x => x.Amount <= 0))
            flag += MenuFactoryErrors.OneOrMoreDishesHaveInvalidAmount;
        if (!flag)
            return new BadRequestResult<Menu>(flag);

        Menu menu = new(request.Name, request.Description);
        foreach(var part in request.Parts)
        {
            var dish = validationData.KnownDishes.First(x => part.Id == x.Id);
            menu.AddMenuPart(new(part.Id, (uint)part.Amount, part.Price, dish.Name));
        }

        return new SuccessResult<Menu>(menu);
    }
}
