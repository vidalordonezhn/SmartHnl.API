using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartHnl.API.Features.Facturacion.DTOs
{
    public class FacturaDetalleDto
    {
        public string? Id { get; set; }
        public string ProductId { get; set; } = null!;
        public string? ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceUnit { get; set; }
        public string TaxType { get; set; } = "GRAVADO";
        public decimal MontoGravado { get; set; }
        public decimal MontoExento { get; set; }
        public decimal MontoExonerado { get; set; }
        public decimal IsvCalculated { get; set; }
        public string? SerialNumber { get; set; }
        public decimal CostUnit { get; set; }
        public string? CustomDescription { get; set; }
        public decimal Discount { get; set; }
    }

    public class FacturaDto
    {
        public string Id { get; set; } = null!;
        public string InvoiceNumber { get; set; } = null!;
        public string DocumentType { get; set; } = "FACTURA";
        public string Date { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string? ClientName { get; set; }
        public string? ClientRtn { get; set; }
        public string Status { get; set; } = "EMITIDA";
        public string PaymentType { get; set; } = "CONTADO";
        public string? PaymentTerm { get; set; }
        public string? DueDate { get; set; }
        public short IsExonerated { get; set; }
        public string? OrdenCompraExenta { get; set; }
        public string? ConstanciaExoneracion { get; set; }
        public string? DocumentoSar { get; set; }
        public decimal SubtotalGravado { get; set; }
        public decimal SubtotalExento { get; set; }
        public decimal SubtotalExonerado { get; set; }
        public decimal IsvTotal { get; set; }
        public decimal TotalGeneral { get; set; }
        public string? Comments { get; set; }
        public string UserId { get; set; } = null!;
        public string? UserName { get; set; }
        public string? CustomClientName { get; set; }
        public string? CustomClientRtn { get; set; }
        public string? CustomClientAddress { get; set; }
        public string? CustomClientPhone { get; set; }
        public string? CustomClientEmail { get; set; }
        public bool Recalcular { get; set; }
        public List<FacturaDetalleDto> Details { get; set; } = new();
    }

    public class FacturaCreateDto
    {
        public string DocumentType { get; set; } = "FACTURA"; // FACTURA (Matriz) o RECIBO_HONORARIOS (Sucursal)
        public string? Date { get; set; }
        [Required]
        public string ClientId { get; set; } = null!;
        public string PaymentType { get; set; } = "CONTADO";
        public string? PaymentTerm { get; set; }
        public string? DueDate { get; set; }
        public short IsExonerated { get; set; } = 0;
        public string? OrdenCompraExenta { get; set; }
        public string? ConstanciaExoneracion { get; set; }
        public string? DocumentoSar { get; set; }
        public string? Comments { get; set; }
        public string? CustomClientName { get; set; }
        public string? CustomClientRtn { get; set; }
        public string? CustomClientAddress { get; set; }
        public string? CustomClientPhone { get; set; }
        public string? CustomClientEmail { get; set; }
        public bool Recalcular { get; set; } = false;
        public List<FacturaDetalleDto> Details { get; set; } = new();
    }
}
