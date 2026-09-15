using System.ComponentModel.DataAnnotations;

namespace SmartHnl.API.Features.Clientes.DTOs
{
    public class ClienteDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Rtn { get; set; } = null!;
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public short ExonerationActive { get; set; }
        public decimal CreditLimit { get; set; }
        public string Status { get; set; } = "ACTIVO";
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }

    public class ClienteCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Rtn { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public short ExonerationActive { get; set; } = 0;
        public decimal CreditLimit { get; set; } = 0.00m;
        public string? Status { get; set; } = "ACTIVO";
        public string? CategoryId { get; set; }
    }

    public class ClienteCategoriaDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
