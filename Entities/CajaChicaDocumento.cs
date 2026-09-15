using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class CajaChicaDocumento : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string DocumentNumber { get; set; } = null!;
        public string PettyCashId { get; set; } = null!;
        public CajaChica? PettyCash { get; set; }
        public string DocumentDate { get; set; } = null!;
        public string Responsible { get; set; } = null!;
        public string Status { get; set; } = "PENDING_REIMBURSEMENT"; // PENDING_REIMBURSEMENT, REIMBURSED, VOIDED
        public string? Notes { get; set; }
        public string? VoidedBy { get; set; }
        public string? VoidReason { get; set; }

        public ICollection<CajaChicaDocumentoItem> Items { get; set; } = new List<CajaChicaDocumentoItem>();
        public ICollection<CajaChicaReembolso> Reembolsos { get; set; } = new List<CajaChicaReembolso>();
    }
}
