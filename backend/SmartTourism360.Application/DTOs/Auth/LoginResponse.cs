namespace SmartTourism360.Application.DTOs.Auth
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = null!;
        public int ExpiresIn { get; set; }
        public UserDto User { get; set; } = null!;
    }
}
