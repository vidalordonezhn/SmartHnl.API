using System.ComponentModel.DataAnnotations;

namespace SmartHnl.API.Features.Usuarios.DTOs
{
    public class UsuarioResponseDto
    {
        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Permissions { get; set; } = "{}";
        public bool Activo { get; set; }
    }

    public class UsuarioCreateDto
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        public string Role { get; set; } = "CAJERO";
        public string Permissions { get; set; } = "{}";
    }

    public class UsuarioUpdateDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Permissions { get; set; }
        public bool? Activo { get; set; }
    }
}
