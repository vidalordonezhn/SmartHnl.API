using System;

namespace SmartHnl.API.Entities
{
    public class Garantia : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? NumeroGarantia { get; set; }
        public string FacturaId { get; set; } = null!;
        public Factura? Factura { get; set; }
        public string ClienteId { get; set; } = null!;
        public Cliente? Cliente { get; set; }
        public string ProductoId { get; set; } = null!;
        public Producto? Producto { get; set; }
        public string DescripcionProducto { get; set; } = "";
        public string NumeroSerie { get; set; } = "";
        public string FechaInicio { get; set; } = "";
        public int DiasGarantia { get; set; } = 0;
        public string FechaVencimiento { get; set; } = "";
        public string Estado { get; set; } = "VIGENTE";
    }
}
