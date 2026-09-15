using System;

namespace SmartHnl.API.Entities
{
    public class ProyectoCostoItem : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProjectCostId { get; set; } = null!;
        public ProyectoCosto? ProjectCost { get; set; }
        public string Date { get; set; } = null!;
        public string Category { get; set; } = null!; // MATERIALES, MANO_DE_OBRA, SUBCONTRATO, TRANSPORTE, OTROS
        public string Description { get; set; } = "";
        public string? SupplierOrResponsible { get; set; }
        public string VoucherType { get; set; } = "FACTURA";
        public string? ReceiptNumber { get; set; }
        public decimal Quantity { get; set; } = 1.00m;
        public decimal UnitPrice { get; set; } = 0.00m;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "EFECTIVO";
        public string? Notes { get; set; }
    }
}
