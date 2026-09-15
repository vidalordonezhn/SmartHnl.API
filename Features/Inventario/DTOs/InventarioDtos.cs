namespace SmartHnl.API.Features.Inventario.DTOs
{
    public class KardexDto
    {
        public string Id { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public string? ProductName { get; set; }
        public string Date { get; set; } = null!;
        public string Type { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal CostUnit { get; set; }
        public decimal StockAfter { get; set; }
        public string Reference { get; set; } = "";
        public string Notes { get; set; } = "";
    }

    public class InventoryAdjustmentDto
    {
        public string ProductId { get; set; } = null!;
        public string Type { get; set; } = "ENTRADA"; // ENTRADA o SALIDA
        public decimal Quantity { get; set; }
        public string? Reason { get; set; }
    }

    public class SerieDto
    {
        public string Id { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public string? ProductName { get; set; }
        public string NumeroSerie { get; set; } = null!;
        public string Estado { get; set; } = "Disponible";
        public string? InvoiceId { get; set; }
        public string? ClientId { get; set; }
        public string? PurchaseId { get; set; }
        public string? ProviderId { get; set; }
        public decimal CostoCompra { get; set; }
        public string? FechaIngreso { get; set; }
        public string? FechaVenta { get; set; }
        public string? UsuarioVenta { get; set; }
        public string? Notas { get; set; }
    }
}
