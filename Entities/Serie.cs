using System;

namespace SmartHnl.API.Entities
{
    public class Serie : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductId { get; set; } = null!;
        public Producto? Product { get; set; }
        public string NumeroSerie { get; set; } = null!;
        public string Estado { get; set; } = "Disponible"; // Disponible, Vendido, etc.
        public string? InvoiceId { get; set; }
        public string? ClientId { get; set; }
        public string? PurchaseId { get; set; }
        public string? ProviderId { get; set; }
        public decimal CostoCompra { get; set; } = 0.00m;
        public string? FechaIngreso { get; set; }
        public string? FechaVenta { get; set; }
        public string? UsuarioVenta { get; set; }
        public string? Notas { get; set; }
    }
}
