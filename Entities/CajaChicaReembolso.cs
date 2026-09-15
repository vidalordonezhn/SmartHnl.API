using System;

namespace SmartHnl.API.Entities
{
    public class CajaChicaReembolso : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? DocumentId { get; set; }
        public CajaChicaDocumento? Document { get; set; }
        public string? PettyCashId { get; set; }
        public CajaChica? PettyCash { get; set; }
        public decimal TotalReceived { get; set; }
        public decimal AmountAppliedToDocument { get; set; }
        public decimal AdditionalAmount { get; set; } = 0.00m;
        public string? CheckNumber { get; set; }
        public string? Reference { get; set; }
        public string ReceivedDate { get; set; } = null!;
        public string Status { get; set; } = "PENDING"; // PENDING, ACCEPTED, VOIDED
        public string? Notes { get; set; }
        public string RegisteredBy { get; set; } = "";
        public string? AcceptedBy { get; set; }
        public string? AcceptedAt { get; set; }
        public string? VoidedBy { get; set; }
        public string? VoidReason { get; set; }
    }
}
