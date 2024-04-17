using Shared.DDD;
using Shared.Patterns.CQRS.Queries;

namespace Shared.Patterns.RepositoryPattern;

public interface IBaseRepository<TEntity> where TEntity : class, IAggregateRoot
{
    public void Create(TEntity entity);
    public void Update(TEntity entity);
    public void Delete(TEntity entity);
    public Task<TEntity> FindByPredicateAsync(Func<TEntity, bool> predicate);
    public Task<IEnumerable<TMapping>> AllAsync<TMapping>(BaseQuery<TEntity, TMapping> query) where TMapping : BaseReadModel;
    public Task<IEnumerable<TMapping>> FindManyByPredicateAsync<TMapping>(Func<TEntity, bool> predicate, BaseQuery<TEntity, TMapping> query) where TMapping : BaseReadModel;
    public Task<bool> IsPredicateTrueAsync(Func<TEntity, bool> predicate);
}
