using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class Cliente : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = null!;
        public string Rtn { get; set; } = null!;
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public short ExonerationActive { get; set; } = 0;
        public decimal CreditLimit { get; set; } = 0.00m;
        public string Status { get; set; } = "ACTIVO";
        public string? CategoryId { get; set; }
        public ClienteCategoria? Category { get; set; }

        public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
        public ICollection<Cotizacion> Cotizaciones { get; set; } = new List<Cotizacion>();
        public ICollection<CuentaPorCobrar> CuentasPorCobrar { get; set; } = new List<CuentaPorCobrar>();
    }
}
