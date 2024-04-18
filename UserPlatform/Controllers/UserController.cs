using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPlatform.Extensions;
using UserPlatform.Models.Internal;
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
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            return this.FromResult(await _userService.UserLoginAsync(request));
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromQuery] RefreshTokenRequest refreshToken)
        {
            return this.FromResult(await _userService.RefreshTokenAsync(refreshToken));
        }

        [HttpDelete]
        public async Task<IActionResult> Logout([FromQuery] string token)
        {
            return this.FromResult(await _userService.UserLogoff(token));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequest request)
        {
            var userId = ClaimHandling.GetUserId(HttpContext);
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            UserUpdateDTO uud = new(userId, request);
            return this.FromResult(await _userService.UpdateUserAsync(uud));
        }
    }
}
