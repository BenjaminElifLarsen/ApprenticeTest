using System.Linq.Expressions;

namespace Shared.Patterns.CQRS.Queries;

public abstract class BaseQuery<TEntity, TMapping> where TEntity : class where TMapping : BaseReadModel
{
    public abstract Expression<Func<TEntity, TMapping>> Map();
}
// https://martinfowler.com/bliki/CQRS.html 10/4/24