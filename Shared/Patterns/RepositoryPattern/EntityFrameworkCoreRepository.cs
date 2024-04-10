﻿using Microsoft.EntityFrameworkCore;
using Shared.DDD;
using Shared.Patterns.CQRS.Queries;

namespace Shared.Patterns.RepositoryPattern;

public class EntityFrameworkCoreRepository<TEntity, TContext> : IBaseRepositroy<TEntity> where TEntity : class, IAggregateRoot where TContext : DbContext
{
    private readonly object _lock;
    private readonly TContext _context; 
    private readonly DbSet<TEntity> _entities;
    public EntityFrameworkCoreRepository(object lockObject, TContext context)
    {
        _lock = lockObject;        
        _context = context;
        _entities = _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TMapping>> AllAsync<TMapping>(BaseQuery<TEntity, TMapping> query) where TMapping : BaseReadModel
    {
        return (await _entities.ToArrayAsync()).AsQueryable().Select(query.Map());
    }

    public void Create(TEntity entity)
    {
        _entities.Add(entity);
    }

    public void Delete(TEntity entity)
    {
        _entities.Remove(entity);
    }

    public async Task<TEntity> FindByPredicateAsync(Func<TEntity, bool> predicate)
    { 
        return (await _entities.ToArrayAsync()).FirstOrDefault(predicate)!;
    }

    public async Task<IEnumerable<TMapping>> FindManyByPredicate<TMapping>(Func<TEntity, bool> predicate, BaseQuery<TEntity, TMapping> query) where TMapping : BaseReadModel
    {
        return (await _entities.ToArrayAsync()).Where(predicate).AsQueryable().Select(query.Map());
    }

    public async Task<bool> IsPredicateTrueAsync(Func<TEntity, bool> predicate)
    {
        return (await _entities.ToArrayAsync()).Any(x => predicate(x));
    }

    public void Update(TEntity entity)
    {
        _entities.Update(entity);
    }
}
