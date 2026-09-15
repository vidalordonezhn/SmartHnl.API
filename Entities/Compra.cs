using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class Compra : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PurchaseNumber { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string ProviderId { get; set; } = null!;
        public Proveedor? Provider { get; set; }
        public string PaymentType { get; set; } = "CONTADO";
        public string? PaymentTerm { get; set; }
        public decimal TotalGeneral { get; set; }
        public string? Comments { get; set; }
        public string Status { get; set; } = "RECIBIDA";

        public ICollection<CompraDetalle> Details { get; set; } = new List<CompraDetalle>();
    }
}
