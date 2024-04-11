using Catering.Shared.IPL.Context;
using Catering.Shared.IPL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CateringDataProcessingPlatform.IPL;

internal sealed class ContextEFFactory : IDesignTimeDbContextFactory<CateringContext>, IContextFactory
{
    public CateringContext CreateDbContext(string[] args)
    {
        var connectionString = "Server=host.docker.internal,1433;Initial Catalog=Catering;User Id=SA; Password=Test123!;TrustServerCertificate=True;";//args[0];
        DbContextOptionsBuilder<CateringContext> optionsBuilder = new DbContextOptionsBuilder<CateringContext>()
            .UseSqlServer(connectionString);
        return new CateringContext(optionsBuilder.Options);
    }

    public IUnitOfWork CreateUnitOfWork(CateringContext context)
    {
        return new UnitOfWorkEFCore(context);
    }
}
