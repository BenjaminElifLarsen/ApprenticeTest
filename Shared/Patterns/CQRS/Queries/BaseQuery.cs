using System.Linq.Expressions;

namespace Shared.Patterns.CQRS.Queries;

public abstract class BaseQuery<TEntity, TMapping> where TEntity : class where TMapping : BaseReadModel
{
    public abstract Expression<Func<TEntity, TMapping>> Map();
}
// https://martinfowler.com/bliki/CQRS.html 10/4/24

public abstract class BaseQuery<TEntity1, TEntity2, TMapping> 
    where TEntity1 : class 
    where TEntity2 : class
    where TMapping : BaseReadModel
{ // Consider if this could be useful to combinate multiple entities into one readmodel. Would not work with reading from repos directly of course
    //but maybe something like ApiRabbitDataProcessing.Process(GetFutureOrdersCommand command) could benefit from it? 
    //the base one can easily be used with IENumerable<TEntity>, unsure how to do something like with those below
    //not sure if the added complicity would be worth it. M. Fowler states CQRS that CQRS does add risky complexity.
    public abstract Expression<Func<TEntity1, TEntity2, TMapping>> Map();
}

public abstract class BaseQuery<TEntity1, TEntity2, TEntity3, TMapping> 
    where TEntity1 : class
    where TEntity2 : class
    where TEntity3 : class
    where TMapping : BaseReadModel
{ 
    public abstract Expression<Func<TEntity1, TEntity2, TEntity3, TMapping>> Map();
}