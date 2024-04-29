using Catering.Shared.IPL.Context;
using Catering.Shared.IPL.UnitOfWork;

namespace Catering.DataProcessingPlatform.IPL;

internal interface IContextFactory
{
    public CateringContext CreateDbContext(string connectionString);
    //public IUnitOfWork CreateUnitOfWork(CateringContext context);
    public IUnitOfWork CreateUnitOfWork(string connectionString);
}
