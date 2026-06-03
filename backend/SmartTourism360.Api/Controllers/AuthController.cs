using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Auth;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return StatusCode(401, ApiResponse<object>.FailureResponse("Phiên đăng nhập không hợp lệ hoặc đã hết hạn."));
            }

            var result = await _authService.GetMeAsync(userId);
            if (!result.Success)
            {
                return StatusCode(401, ApiResponse<object>.FailureResponse(result.Message));
            }

            return Success(result.Data, result.Message);
        }
    }
}
