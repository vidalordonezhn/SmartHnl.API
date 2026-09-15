using System.ComponentModel.DataAnnotations;

namespace SmartHnl.API.Features.Proveedores.DTOs
{
    public class ProveedorDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Rtn { get; set; } = null!;
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string ContactName { get; set; } = "";
    }

    public class ProveedorCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Rtn { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? ContactName { get; set; }
    }
}
