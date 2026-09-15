using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class AutorizacionCaiBoletaCompra : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Cai { get; set; } = null!;
        public string RangoInicial { get; set; } = null!;
        public string RangoFinal { get; set; } = null!;
        public string CorrelativoActual { get; set; } = null!;
        public string FechaLimite { get; set; } = null!;
        public short Activo { get; set; } = 1;
        public ICollection<BoletaCompra> BoletasCompra { get; set; } = new List<BoletaCompra>();
    }
}
