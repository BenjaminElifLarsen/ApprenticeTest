using Shared.Patterns.RepositoryPattern;
using UserPlatform.Shared.IPL.Repository.Interfaces;

namespace UserPlatform.Shared.IPL.UnitOfWork;

public interface IUnitOfWork : IBaseUnitOfWork
{
    public IUserRepository UserRepository { get; }
}
