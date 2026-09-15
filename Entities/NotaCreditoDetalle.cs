using System;

namespace SmartHnl.API.Entities
{
    public class NotaCreditoDetalle : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CreditNoteId { get; set; } = null!;
        public NotaCredito? CreditNote { get; set; }
        public string? ProductId { get; set; }
        public Producto? Product { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceUnit { get; set; }
        public string TaxType { get; set; } = "GRAVADO";
        public decimal MontoGravado { get; set; }
        public decimal MontoExento { get; set; }
        public decimal MontoExonerado { get; set; }
        public decimal IsvCalculated { get; set; }
    }
}
