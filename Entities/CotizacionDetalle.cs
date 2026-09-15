using System;

namespace SmartHnl.API.Entities
{
    public class CotizacionDetalle : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string QuotationId { get; set; } = null!;
        public Cotizacion? Quotation { get; set; }
        public string ProductId { get; set; } = null!;
        public Producto? Product { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceUnit { get; set; }
        public string TaxType { get; set; } = "GRAVADO";
        public decimal MontoGravado { get; set; }
        public decimal MontoExento { get; set; }
        public decimal MontoExonerado { get; set; }
        public decimal IsvCalculated { get; set; }
        public string? CustomDescription { get; set; }
        public decimal Discount { get; set; } = 0.00m;
    }
}
