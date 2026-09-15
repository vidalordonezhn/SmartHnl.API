using System;

namespace SmartHnl.API.Entities
{
    public class Kardex : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductId { get; set; } = null!;
        public Producto? Product { get; set; }
        public string Date { get; set; } = null!;
        public string Type { get; set; } = null!; // ENTRADA, SALIDA, VENTA, COMPRA, AJUSTE, RECALCULADO
        public decimal Quantity { get; set; }
        public decimal CostUnit { get; set; }
        public decimal StockAfter { get; set; }
        public string Reference { get; set; } = "";
        public string Notes { get; set; } = "";
    }
}
