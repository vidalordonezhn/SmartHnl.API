using System;

namespace SmartHnl.API.Entities
{
    public class CajaChicaGasto : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PettyCashId { get; set; } = null!;
        public CajaChica? PettyCash { get; set; }
        public string AccountId { get; set; } = null!;
        public CajaChicaCuenta? Account { get; set; }
        public string? CreditorId { get; set; }
        public CajaChicaAcreedor? Creditor { get; set; }
        public string ExpenseDate { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ReceiptNumber { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "PENDING"; // PENDING, LIQUIDATED, VOIDED
        public string? Notes { get; set; }
        public string? VoidedBy { get; set; }
        public string? VoidReason { get; set; }
    }
}
