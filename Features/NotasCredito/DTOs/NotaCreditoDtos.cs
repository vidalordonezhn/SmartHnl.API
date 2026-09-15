using System.Collections.Generic;

namespace SmartHnl.API.Features.NotasCredito.DTOs
{
    public class NotaCreditoDto
    {
        public string Id { get; set; } = null!;
        public string CreditNoteNumber { get; set; } = null!;
        public string CaiId { get; set; } = null!;
        public string CaiCode { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string? ClientName { get; set; }
        public string InvoiceId { get; set; } = null!;
        public string InvoiceNumber { get; set; } = null!;
        public string NoteType { get; set; } = "ANULACION";
        public string Comments { get; set; } = "";
        public decimal SubtotalGravado { get; set; }
        public decimal SubtotalExento { get; set; }
        public decimal SubtotalExonerado { get; set; }
        public decimal IsvTotal { get; set; }
        public decimal TotalGeneral { get; set; }
        public string Status { get; set; } = "EMITIDA";
    }

    public class CaiNotaCreditoDto
    {
        public string Id { get; set; } = null!;
        public string Cai { get; set; } = null!;
        public string RangoInicial { get; set; } = null!;
        public string RangoFinal { get; set; } = null!;
        public string CorrelativoActual { get; set; } = null!;
        public string FechaLimite { get; set; } = null!;
        public short Activo { get; set; } = 1;
    }
}
