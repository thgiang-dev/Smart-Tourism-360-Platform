using System;
using System.Threading.Tasks;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Auth;

namespace SmartTourism360.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<UserDto>> GetMeAsync(Guid userId);
    }
}
