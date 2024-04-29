using Catering.Shared.IPL.UnitOfWork;
using Catering.DataProcessingPlatform.AL.Handlers.ApiCommunication.CQRS.Queries;
using Shared.Communication.Models.Dish;
using Shared.Patterns.ResultPattern;

namespace Catering.DataProcessingPlatform.AL.Handlers.ApiCommunication.DataProcessing;

internal partial class ApiRabbitDataProcessing
{
    internal Result<DishListQueryResponse> Process(DishListCommand command)
    {
        _logger.Debug("{Identifier}: Processing request {@DishList}", _identifier, command);
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_configurationManager.GetDatabaseString());
        var dishes = unitOfWork.DishRepository.AllAsync(new DishListQuery()).Result;
        DishListQueryResponse dlqr = new(dishes);
        _logger.Debug("{Identifier}: Processed request {@DishList}", _identifier, command);
        return new SuccessResult<DishListQueryResponse>(dlqr);
    }
}
