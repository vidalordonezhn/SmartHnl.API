using System;

namespace SmartHnl.API.Entities
{
    public class CuentaPorPagar : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PurchaseId { get; set; } = null!;
        public string PurchaseNumber { get; set; } = null!;
        public string ProviderId { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; } = "PENDIENTE";
        public string DueDate { get; set; } = null!;
        public string Payments { get; set; } = "[]";
    }
}
