using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class ProyectoCosto : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Code { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public string? QuotationId { get; set; }
        public Cotizacion? Quotation { get; set; }
        public string? QuotationNumber { get; set; }
        public string? InvoiceId { get; set; }
        public Factura? Invoice { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? ClientId { get; set; }
        public Cliente? Client { get; set; }
        public string ClientName { get; set; } = "";
        public string? ClientRtn { get; set; }
        public string? ClientPhone { get; set; }
        public string? ClientAddress { get; set; }
        public string? Responsible { get; set; }
        public string StartDate { get; set; } = null!;
        public string? EstimatedEndDate { get; set; }
        public string? ClosedDate { get; set; }
        public string? ClosedBy { get; set; }
        public string? ClosingNotes { get; set; }
        public string Status { get; set; } = "ABIERTO"; // ABIERTO, CERRADO, CANCELADO
        public decimal QuotedAmount { get; set; } = 0.00m;
        public decimal InvoicedAmount { get; set; } = 0.00m;
        public string? Notes { get; set; }

        public ICollection<ProyectoCostoItem> Items { get; set; } = new List<ProyectoCostoItem>();
    }
}
