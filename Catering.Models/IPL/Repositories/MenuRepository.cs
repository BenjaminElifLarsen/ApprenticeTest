using Catering.Shared.IPL.Repositories.Interfaces;
using CateringDataProcessingPlatform.DL.Models;
using Shared.Patterns.CQRS.Queries;
using Shared.Patterns.RepositoryPattern;

namespace Catering.Shared.IPL.Repositories;

public sealed class MenuRepository : IMenuRepository
{
    private readonly IBaseRepository<Menu> _repository;

    public MenuRepository(IBaseRepository<Menu> repository)
    {
        _repository = repository;        
    }

    public async Task<IEnumerable<TMapping>> AllAsync<TMapping>(BaseQuery<Menu, TMapping> query) where TMapping : BaseReadModel
    {
        return await _repository.AllAsync(query);
    }

    public void Create(Menu menu)
    {
        _repository.Create(menu);
    }

    public void Delete(Menu menu)
    {
        _repository.Delete(menu);
    }

    public async Task<Menu> GetSingleAsync(Func<Menu, bool> predicate)
    {
        return await _repository.FindByPredicateAsync(predicate);
    }
}
