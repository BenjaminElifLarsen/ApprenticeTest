using Catering.Shared.DL.Factories.MenuFactory;
using Catering.Shared.IPL.UnitOfWork;
using Catering.DataProcessingPlatform.Extensions;
using Shared.Communication.Models.Menu;
using Shared.Patterns.ResultPattern;

namespace Catering.DataProcessingPlatform.AL.Handlers.ApiCommunication.DataProcessing;

internal partial class ApiRabbitDataProcessing
{
    internal Result Process(MenuCreationCommand command)
    {
        _logger.Debug("{Identifier}: Processing request {@MenuCreation}", _identifier, command);
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_configurationManager.GetDatabaseString());
        var usedNames = unitOfWork.MenuRepository.AllAsync(new MenuDataQuery()).Result;
        var permittedDishes = unitOfWork.DishRepository.AllAsync(new MenuDishDataQuery()).Result;
        MenuValidationData mvd = new(usedNames, permittedDishes);
        var factory = _factories.MenuFactory;
        var result = factory.Build(command.ToRequest(), mvd);
        if (!result)
        {
            _logger.Warning("{Identifier}: Failed at creating menu {@MenuCreation} - {Errors}", _identifier, command, result);
        }
        else
        {
            unitOfWork.MenuRepository.Create(result.Data);
            unitOfWork.Commit();
        }
        _logger.Debug("{Identifier}: Processed request {@MenuCreation}", _identifier, command);
        return result.ToGeneric();
    }
}
