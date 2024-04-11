using Catering.Shared.DL.Communication.Models;
using Catering.Shared.DL.Factories.DishFactory;
using Catering.Shared.IPL.UnitOfWork;
using CateringDataProcessingPlatform.AL.Services.Contracts;
using CateringDataProcessingPlatform.DL.Models;
using Serilog;
using Shared.Patterns.CQRS.Queries;
using Shared.Service;
using System.Linq.Expressions;

namespace CateringDataProcessingPlatform.AL.Services.SeederService;

internal sealed partial class SeederService : BaseService, ISeederService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    public SeederService(IUnitOfWork unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork;        
        _logger = logger;
    }

    public void Seed()
    {
        var dishes = _unitOfWork.DishRepository.AllAsync(new DishCheckQuery()).Result;
        if (!dishes.Any())
        {
            _logger.Warning("{Identifier}: Found no dishes. Seeding", _identifier);
            DishFactory dishFactory = new();
            DishValidationData dvd = new([]);
            var dish1 = dishFactory.Build(new DishCreationRequest { Name = "Æg" }, dvd).Data;
            var dish2 = dishFactory.Build(new DishCreationRequest { Name = "Kød" }, dvd).Data;
            var dish3 = dishFactory.Build(new DishCreationRequest { Name = "Rygbrød" }, dvd).Data;
            var dish4 = dishFactory.Build(new DishCreationRequest { Name = "Argurk" }, dvd).Data;
            _unitOfWork.DishRepository.Create(dish1);
            _unitOfWork.DishRepository.Create(dish2);
            _unitOfWork.DishRepository.Create(dish3);
            _unitOfWork.DishRepository.Create(dish4);
            _unitOfWork.Commit();
            dishes = [new(dish1.Id), new(dish2.Id), new(dish3.Id), new(dish4.Id)];
        }
        _logger.Information("{Identifier}: The following items are present in the context: {Dishes}", _identifier, dishes.Select(x => x.Id).ToArray());

    }




    private class DishCheck : BaseReadModel
    {
        public Guid Id { get; private set; }

        public DishCheck(Guid id)
        {
            Id = id;            
        }
    }

    private class DishCheckQuery : BaseQuery<Dish, DishCheck>
    {
        public override Expression<Func<Dish, DishCheck>> Map()
        {
            return e => new(e.Id);
        }
    }
}

