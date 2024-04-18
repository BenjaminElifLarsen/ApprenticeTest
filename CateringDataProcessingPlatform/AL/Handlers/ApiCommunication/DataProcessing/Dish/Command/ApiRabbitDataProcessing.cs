using Catering.Shared.DL.Factories.DishFactory;
using Catering.Shared.IPL.UnitOfWork;
using CateringDataProcessingPlatform.Extensions;
using Shared.Communication.Models.Dish;
using Shared.Patterns.ResultPattern;

namespace CateringDataProcessingPlatform.AL.Handlers.ApiCommunication.DataProcessing;

internal partial class ApiRabbitDataProcessing
{
    internal Result Process(DishCreationCommand command)
    {
        _logger.Debug("{Identifier}: Processing request {@DishCreation}", _identifier, command);
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_configurationManager.GetDatabaseString());
        var existingDishes = unitOfWork.DishRepository.AllAsync(new DishDataQuery()).Result;
        DishValidationData dvd = new(existingDishes);
        var factory = _factories.DishFactory;
        var result = factory.Build(command.ToRequest(), dvd);
        if (!result)
        {
            _logger.Warning("{Identifier}: failed at creating dish {@DishCreation} - {Errors}", _identifier, command, result);
        }
        else
        {
            unitOfWork.DishRepository.Create(result.Data);
            unitOfWork.Commit();
        }
        _logger.Debug("{Identifier}: Processed request {@DishCreation}", _identifier, command);
        return result.ToGeneric();
    }
}
