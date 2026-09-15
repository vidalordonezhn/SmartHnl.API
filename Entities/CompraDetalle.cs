using System;

namespace SmartHnl.API.Entities
{
    public class CompraDetalle : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PurchaseId { get; set; } = null!;
        public Compra? Purchase { get; set; }
        public string ProductId { get; set; } = null!;
        public Producto? Product { get; set; }
        public decimal Quantity { get; set; }
        public decimal CostUnit { get; set; }
    }
}
