using Catering.Shared.IPL.Context;
using Catering.Shared.IPL.UnitOfWork;

namespace CateringDataProcessingPlatform.IPL;

internal interface IContextFactory
{
    public CateringContext CreateDbContext(string[] args);
    public IUnitOfWork CreateUnitOfWork(CateringContext context);
}
