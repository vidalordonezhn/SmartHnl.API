using System.Collections.Generic;
using SmartHnl.API.Features.Facturacion.DTOs;

namespace SmartHnl.API.Features.Cotizaciones.DTOs
{
    public class CotizacionDto
    {
        public string Id { get; set; } = null!;
        public string QuotationNumber { get; set; } = null!;
        public string DocumentType { get; set; } = "FACTURA";
        public string Date { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string? ClientName { get; set; }
        public string? ClientRtn { get; set; }
        public string Status { get; set; } = "PENDIENTE";
        public int ValidityDays { get; set; } = 15;
        public string PaymentType { get; set; } = "CONTADO";
        public string? PaymentTerm { get; set; }
        public short IsExonerated { get; set; }
        public decimal SubtotalGravado { get; set; }
        public decimal SubtotalExento { get; set; }
        public decimal SubtotalExonerado { get; set; }
        public decimal IsvTotal { get; set; }
        public decimal TotalGeneral { get; set; }
        public string? Comments { get; set; }
        public string UserId { get; set; } = null!;
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
        public List<FacturaDetalleDto> Details { get; set; } = new();
    }
}
