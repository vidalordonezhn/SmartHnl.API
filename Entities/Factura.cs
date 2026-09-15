using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class Factura : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string InvoiceNumber { get; set; } = null!;
        public string DocumentType { get; set; } = "FACTURA"; // FACTURA (Matriz) o RECIBO_HONORARIOS (Sucursal)
        public string Date { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public Cliente? Client { get; set; }
        public string Status { get; set; } = "EMITIDA"; // EMITIDA, ANULADA
        public string PaymentType { get; set; } = "CONTADO"; // CONTADO, CREDITO
        public string? PaymentTerm { get; set; }
        public string? DueDate { get; set; }
        public short IsExonerated { get; set; } = 0;
        public string? OrdenCompraExenta { get; set; }
        public string? ConstanciaExoneracion { get; set; }
        public string? DocumentoSar { get; set; }
        public decimal SubtotalGravado { get; set; }
        public decimal SubtotalExento { get; set; }
        public decimal SubtotalExonerado { get; set; }
        public decimal IsvTotal { get; set; }
        public decimal TotalGeneral { get; set; }
        public string? Comments { get; set; }
        public string UserId { get; set; } = null!;
        public Usuario? User { get; set; }
        public string? CustomClientName { get; set; }
        public string? CustomClientRtn { get; set; }
        public string? CustomClientAddress { get; set; }
        public string? CustomClientPhone { get; set; }
        public string? CustomClientEmail { get; set; }
        public bool Recalcular { get; set; } = false;

        public ICollection<FacturaDetalle> Details { get; set; } = new List<FacturaDetalle>();
    }
}
