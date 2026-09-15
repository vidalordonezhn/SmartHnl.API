using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class Cotizacion : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string QuotationNumber { get; set; } = null!;
        public string DocumentType { get; set; } = "FACTURA";
        public string Date { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public Cliente? Client { get; set; }
        public string Status { get; set; } = "PENDIENTE";
        public int ValidityDays { get; set; } = 15;
        public string PaymentType { get; set; } = "CONTADO";
        public string? PaymentTerm { get; set; }
        public short IsExonerated { get; set; } = 0;
        public decimal SubtotalGravado { get; set; }
        public decimal SubtotalExento { get; set; }
        public decimal SubtotalExonerado { get; set; }
        public decimal IsvTotal { get; set; }
        public decimal TotalGeneral { get; set; }
        public string? Comments { get; set; }
        public string UserId { get; set; } = null!;
        public Usuario? User { get; set; }
        public string? InvoiceId { get; set; }
        public string? ApprovedBy { get; set; }
        public string? ApprovedAt { get; set; }
        public string? RejectedBy { get; set; }
        public string? RejectedAt { get; set; }
        public string? RejectionReason { get; set; }
        public string? CustomClientName { get; set; }
        public string? CustomClientRtn { get; set; }
        public string? CustomClientAddress { get; set; }
        public string? CustomClientPhone { get; set; }
        public string? CustomClientEmail { get; set; }

        public ICollection<CotizacionDetalle> Details { get; set; } = new List<CotizacionDetalle>();
    }
}
