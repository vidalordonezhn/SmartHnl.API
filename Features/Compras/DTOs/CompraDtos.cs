using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartHnl.API.Features.Compras.DTOs
{
    public class CompraDetalleDto
    {
        public string? Id { get; set; }
        public string ProductId { get; set; } = null!;
        public string? ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal CostUnit { get; set; }
        public decimal Total => Quantity * CostUnit;
    }

    public class CompraDto
    {
        public string Id { get; set; } = null!;
        public string PurchaseNumber { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string ProviderId { get; set; } = null!;
        public string? ProviderName { get; set; }
        public string? ProviderRtn { get; set; }
        public string PaymentType { get; set; } = "CONTADO";
        public string? PaymentTerm { get; set; }
        public decimal TotalGeneral { get; set; }
        public string? Comments { get; set; }
        public string Status { get; set; } = "RECIBIDA";
        public List<CompraDetalleDto> Details { get; set; } = new();
    }

    public class CompraCreateDto
    {
        [Required]
        public string PurchaseNumber { get; set; } = null!;
        public string? Date { get; set; }
        [Required]
        public string ProviderId { get; set; } = null!;
        public string PaymentType { get; set; } = "CONTADO";
        public string? PaymentTerm { get; set; }
        public string? Comments { get; set; }
        public List<CompraDetalleDto> Details { get; set; } = new();
    }
}
