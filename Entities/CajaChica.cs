using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class CajaChica : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = null!;
        public decimal BaseAmount { get; set; } = 0.00m;
        public decimal MinLimit { get; set; } = 0.00m;
        public string Responsible { get; set; } = null!;
        public string? UserId { get; set; }
        public string FundType { get; set; } = "CHECK";
        public string? BankId { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }
        public string Status { get; set; } = "ACTIVE";
        public string? Notes { get; set; }

        public ICollection<CajaChicaGasto> Gastos { get; set; } = new List<CajaChicaGasto>();
        public ICollection<CajaChicaDocumento> Documentos { get; set; } = new List<CajaChicaDocumento>();
        public ICollection<CajaChicaReembolso> Reembolsos { get; set; } = new List<CajaChicaReembolso>();
        public ICollection<CajaChicaMovimiento> Movimientos { get; set; } = new List<CajaChicaMovimiento>();
    }
}
