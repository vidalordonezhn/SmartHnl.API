using System;

namespace SmartHnl.API.Entities
{
    public class CajaChicaDocumentoItem : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string DocumentId { get; set; } = null!;
        public CajaChicaDocumento? Document { get; set; }
        public string ExpenseId { get; set; } = null!;
        public CajaChicaGasto? Expense { get; set; }
    }
}
