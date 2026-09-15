using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class BoletaCompra : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string BoletaNumber { get; set; } = null!;
        public string CaiId { get; set; } = null!;
        public AutorizacionCaiBoletaCompra? Cai { get; set; }
        public string CaiCode { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string ProviderId { get; set; } = null!;
        public Acreedor? Provider { get; set; }
        public string ProviderName { get; set; } = null!;
        public string ProviderRtn { get; set; } = null!;
        public string? ProviderPhone { get; set; }
        public string? ProviderAddress { get; set; }
        public string PaymentType { get; set; } = "CONTADO";
        public string? Comments { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalGeneral { get; set; }
        public short IsExonerated { get; set; } = 0;
        public string? OrdenCompraExenta { get; set; }
        public string? ConstanciaExoneracion { get; set; }
        public string? RegistroSag { get; set; }
        public string UserId { get; set; } = null!;
        public Usuario? User { get; set; }
        public string Status { get; set; } = "EMITIDA";

        public ICollection<BoletaCompraDetalle> Details { get; set; } = new List<BoletaCompraDetalle>();
    }
}
