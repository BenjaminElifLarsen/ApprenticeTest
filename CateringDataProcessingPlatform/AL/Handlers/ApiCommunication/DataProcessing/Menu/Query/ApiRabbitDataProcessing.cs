using Catering.Shared.IPL.UnitOfWork;
using CateringDataProcessingPlatform.AL.Handlers.ApiCommunication.CQRS.Queries;
using Shared.Communication.Models.Menu;
using Shared.Patterns.ResultPattern;

namespace CateringDataProcessingPlatform.AL.Handlers.ApiCommunication.DataProcessing;

internal partial class ApiRabbitDataProcessing
{
    internal Result<MenuListFinerDetailsQueryResponse> Process(MenuListCommand command)
    {
        _logger.Debug("{Identifier}: Processing request {@MenuList}", _identifier, command);
        IUnitOfWork unitOfWork = _contextFactory.CreateUnitOfWork(_configurationManager.GetDatabaseString());
        var menues = unitOfWork.MenuRepository.AllAsync(new MenuListMenuQuery()).Result;
        MenuListFinerDetailsQueryResponse mlfdqr = new(menues);
        _logger.Debug("{Identifier}: Processed request {@MenuList}", _identifier, command);
        return new SuccessResult<MenuListFinerDetailsQueryResponse>(mlfdqr);
    }
}
