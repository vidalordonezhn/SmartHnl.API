using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class NotaCredito : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CreditNoteNumber { get; set; } = null!;
        public string CaiId { get; set; } = null!;
        public AutorizacionCaiNotaCredito? Cai { get; set; }
        public string CaiCode { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public Cliente? Client { get; set; }
        public string InvoiceId { get; set; } = null!;
        public Factura? Invoice { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public string NoteType { get; set; } = "ANULACION";
        public string Comments { get; set; } = "";
        public decimal SubtotalGravado { get; set; }
        public decimal SubtotalExento { get; set; }
        public decimal SubtotalExonerado { get; set; }
        public decimal IsvTotal { get; set; }
        public decimal TotalGeneral { get; set; }
        public string UserId { get; set; } = null!;
        public Usuario? User { get; set; }
        public string Status { get; set; } = "EMITIDA";

        public ICollection<NotaCreditoDetalle> Details { get; set; } = new List<NotaCreditoDetalle>();
    }
}
