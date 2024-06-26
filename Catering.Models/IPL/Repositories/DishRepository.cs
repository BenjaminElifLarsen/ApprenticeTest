﻿using Catering.Shared.IPL.Repositories.Interfaces;
using Catering.Shared.DL.Models;
using Shared.Patterns.CQRS.Queries;
using Shared.Patterns.RepositoryPattern;

namespace Catering.Shared.IPL.Repositories;

public sealed class DishRepository : IDishRepository
{
    private readonly IBaseRepository<Dish> _repository;

    public DishRepository(IBaseRepository<Dish> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TMapping>> AllAsync<TMapping>(BaseQuery<Dish, TMapping> query) where TMapping : BaseReadModel
    {
        return await _repository.AllAsync(query);
    }

    public void Create(Dish dish)
    {
        _repository.Create(dish);
    }

    public void Delete(Dish dish)
    {
        _repository.Delete(dish);
    }

    public async Task<Dish> GetSingleAsync(Func<Dish, bool> predicate)
    {
        return await _repository.FindByPredicateAsync(predicate);
    }

    public async Task<bool> IsNameUnique(string name)
    {
        return !(await _repository.IsPredicateTrueAsync(x => x.Name == name));
    }

    public void Update(Dish dish)
    {
        _repository.Update(dish);
    }

    //public async Task<TMapping> GetSingleAsync<TMapping>(Func<Dish, bool> predicate, BaseQuery<Dish, TMapping> query) where TMapping : BaseReadModel
    //{
    //    return await _repository.FindByPredicateAsync()
    //}
}
