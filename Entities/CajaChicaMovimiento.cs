using System;

namespace SmartHnl.API.Entities
{
    public class CajaChicaMovimiento : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PettyCashId { get; set; } = null!;
        public CajaChica? PettyCash { get; set; }
        public string MovementType { get; set; } = null!; // INGRESO, EGRESO, APERTURA, REEMBOLSO
        public decimal Amount { get; set; }
        public string MovementDate { get; set; } = null!;
        public string? ReferenceType { get; set; }
        public string? ReferenceId { get; set; }
        public string Description { get; set; } = "";
    }
}
