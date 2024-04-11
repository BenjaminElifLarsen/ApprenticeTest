using Shared.Patterns.RepositoryPattern;
using UserPlatform.Shared.DL.Models;
using UserPlatform.Shared.IPL.Context;
using UserPlatform.Shared.IPL.Repository;
using UserPlatform.Shared.IPL.Repository.Interfaces;

namespace UserPlatform.Shared.IPL.UnitOfWork;

public sealed class UnitOfWorkEFCore : IUnitOfWork
{
    private readonly UserContext _context;
    private readonly IUserRepository _userRepository;

    public UnitOfWorkEFCore(UserContext contact)
    {
        _context = contact;
        _userRepository = new UserRepository(new EntityFrameworkCoreRepository<User, UserContext>(_context));        
    }

    public IUserRepository UserRepository => _userRepository;

    public void Commit()
    {
        _context.SaveChanges();
    }
}
