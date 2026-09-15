namespace SmartHnl.API.Features.Auth.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Permissions { get; set; } = "{}";
    }
}
