using System.Collections.Generic;

namespace SmartHnl.API.Features.BoletasCompra.DTOs
{
    public class BoletaDetalleDto
    {
        public string? Id { get; set; }
        public string Description { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal PriceUnit { get; set; }
        public decimal TotalItem { get; set; }
    }

    public class BoletaCompraDto
    {
        public string Id { get; set; } = null!;
        public string BoletaNumber { get; set; } = null!;
        public string CaiId { get; set; } = null!;
        public string CaiCode { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string ProviderId { get; set; } = null!;
        public string ProviderName { get; set; } = null!;
        public string ProviderRtn { get; set; } = null!;
        public string? ProviderPhone { get; set; }
        public string? ProviderAddress { get; set; }
        public string PaymentType { get; set; } = "CONTADO";
        public string? Comments { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalGeneral { get; set; }
        public short IsExonerated { get; set; }
        public string Status { get; set; } = "EMITIDA";
        public List<BoletaDetalleDto> Details { get; set; } = new();
    }

    public class CaiBoletaCompraDto
    {
        public string Id { get; set; } = null!;
        public string Cai { get; set; } = null!;
        public string RangoInicial { get; set; } = null!;
        public string RangoFinal { get; set; } = null!;
        public string CorrelativoActual { get; set; } = null!;
        public string FechaLimite { get; set; } = null!;
        public short Activo { get; set; } = 1;
    }
}
