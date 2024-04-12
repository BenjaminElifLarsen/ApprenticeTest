using Shared.Service;
using UserPlatform.Communication.Contracts;
using UserPlatform.Services.Contracts;
using UserPlatform.Shared.DL.Factories;
using UserPlatform.Shared.IPL.UnitOfWork;
using ILogger = Serilog.ILogger;

namespace UserPlatform.Services.UserService;

internal sealed partial class UserService : BaseService, IUserService
{
    private readonly ISecurityService _securityService;
    private readonly ICommunication _communication;
    private readonly IUserFactory _userFactory;
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(ISecurityService securityService, ICommunication communication, IUserFactory userFactory, IUnitOfWork unitOfWork, ILogger logger)
    {
        _securityService = securityService;
        _communication = communication;
        _userFactory = userFactory;
        _unitOfWork = unitOfWork;
        _logger = logger;        
    }
}
