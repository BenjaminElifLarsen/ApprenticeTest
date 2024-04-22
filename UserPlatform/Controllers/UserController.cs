using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UserPlatform.Extensions;
using UserPlatform.Helpers;
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
    public class UserController : BaseApiController
    {
        private IUserService _userService;

        public UserController(IUserService userService, ILogger logger) : base(logger)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationRequest request)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var result = this.FromResult(await _userService.CreateUserAsync(request));
            stopwatch.Stop();
            _logger.Information("{Identifier}: Create user took {Time}", _identifier, stopwatch.Elapsed); // In reality, would only log endpoint calls if they took mover than a specified time
            return result;
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
