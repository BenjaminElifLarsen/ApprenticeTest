using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPlatform.Extensions;
using UserPlatform.Models.User.Requests;
using UserPlatform.Services.Contracts;
using UserPlatform.Shared.Communication.Models;
using UserPlatform.Sys;
using ILogger = Serilog.ILogger;

namespace UserPlatform.Controllers
{
    [Authorize(Policy = AccessLevels.DEFAULT_USER)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private ILogger _logger;

        public UserController(IUserService userService, ILogger logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationRequest request)
        {
            return this.FromResult(await _userService.CreateUserAsync(request));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login([FromQuery] UserLoginRequest request)
        {
            return this.FromResult(await _userService.UserLoginAsync(request));
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromQuery] RefreshTokenRequest refreshToken)
        {
            return this.FromResult(await _userService.RefreshTokenAsync(refreshToken));
        } // TODO: rememeber method to revoke refresh token (Logout)
    }
}
