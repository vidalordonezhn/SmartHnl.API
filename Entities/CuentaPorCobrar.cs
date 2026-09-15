using System;

namespace SmartHnl.API.Entities
{
    public class CuentaPorCobrar : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string InvoiceId { get; set; } = null!;
        public Factura? Invoice { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public Cliente? Client { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; } = "PENDIENTE";
        public string DueDate { get; set; } = null!;
        public string Payments { get; set; } = "[]"; // Historial de abonos en JSON
    }
}
