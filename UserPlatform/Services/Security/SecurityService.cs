using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Shared.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserPlatform.Services.Contracts;
using UserPlatform.Shared.DL.Factories.RefreshTokenFactory;
using UserPlatform.Shared.DL.Models;
using UserPlatform.Shared.Helpers;
using UserPlatform.Shared.IPL.UnitOfWork;
using UserPlatform.Sys;
using UserPlatform.Sys.Appsettings.Models;
using ILogger = Serilog.ILogger;

namespace UserPlatform.Services.Security;

internal sealed partial class SecurityService : BaseService, ISecurityService
{
    private JwtSettings _jwtSettings; // Not readonly to make it such that the values can be updated in runtime (with the correct setup)
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtSecurityTokenHandler _jwtHandler;
    private readonly byte[] _jwtKey;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenFactory _refreshTokenFactory;

    public SecurityService(IOptions<JwtSettings> jwtSettings, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IRefreshTokenFactory refreshTokenFactory, ILogger logger) : base(logger)
    {
        _jwtSettings = jwtSettings.Value;
        _unitOfWork = unitOfWork;
        _jwtHandler = new JwtSecurityTokenHandler();
        _jwtKey = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        _passwordHasher = passwordHasher;
        _refreshTokenFactory = refreshTokenFactory;
    }

    private string CreateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(_jwtKey);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // TODO: explain what these are
            new(ClaimTypes.NameIdentifier, user.CompanyName),
            new("level", AccessLevels.DEFAULT_USER),
        };

        var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, null!, DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireDurationMinutes), credentials);
        return _jwtHandler.WriteToken(token);
    }

    private string CreateRefreshToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(_jwtKey);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.CompanyName),
        };

        var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, null!, DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshExpireDurationMinutes), credentials);
        return _jwtHandler.WriteToken(token);
    }
}
