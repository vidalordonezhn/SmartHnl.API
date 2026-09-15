using System;

namespace SmartHnl.API.Entities
{
    public class BoletaCompraDetalle : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string BoletaId { get; set; } = null!;
        public BoletaCompra? Boleta { get; set; }
        public string Description { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal PriceUnit { get; set; }
        public decimal TotalItem { get; set; }
    }
}
