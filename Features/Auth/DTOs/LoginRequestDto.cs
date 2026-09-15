using System.ComponentModel.DataAnnotations;

namespace SmartHnl.API.Features.Auth.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
