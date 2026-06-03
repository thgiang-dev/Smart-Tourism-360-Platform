using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Auth;
using SmartTourism360.Application.Interfaces;
using SmartTourism360.Infrastructure.Data;

namespace SmartTourism360.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly SmartTourism360DbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(SmartTourism360DbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return ApiResponse<LoginResponse>.FailureResponse("Tài khoản hoặc mật khẩu không chính xác.");
            }

            if (user.Status != "active")
            {
                return ApiResponse<LoginResponse>.FailureResponse($"Tài khoản đang bị khóa hoặc ngưng hoạt động (Trạng thái: {user.Status}).");
            }

            // Cấu hình JWT Settings từ appsettings
            var jwtSettings = _configuration.GetSection("Jwt");
            var keyString = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
            var key = Encoding.UTF8.GetBytes(keyString);
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var durationMinutes = int.Parse(jwtSettings["DurationInMinutes"] ?? "60");

            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.Code)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(durationMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Cập nhật LastLoginAt
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var response = new LoginResponse
            {
                AccessToken = tokenString,
                ExpiresIn = durationMinutes * 60,
                User = new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role.Name
                }
            };

            return ApiResponse<LoginResponse>.SuccessResponse(response, "Đăng nhập thành công.");
        }

        public async Task<ApiResponse<UserDto>> GetMeAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return ApiResponse<UserDto>.FailureResponse("Không tìm thấy thông tin người dùng.");
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.Name
            };

            return ApiResponse<UserDto>.SuccessResponse(userDto, "Lấy thông tin tài khoản thành công.");
        }
    }
}
