using System.Collections.Generic;

namespace SmartHnl.API.Features.CosteoObras.DTOs
{
    public class ProyectoCostoItemDto
    {
        public string? Id { get; set; }
        public string ProjectCostId { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string Category { get; set; } = "MATERIALES";
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

    public class ProyectoCostoDto
    {
        public string Id { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public string? QuotationId { get; set; }
        public string? QuotationNumber { get; set; }
        public string? InvoiceId { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? ClientId { get; set; }
        public string ClientName { get; set; } = "";
        public string? ClientRtn { get; set; }
        public string? ClientPhone { get; set; }
        public string? ClientAddress { get; set; }
        public string? Responsible { get; set; }
        public string StartDate { get; set; } = null!;
        public string? EstimatedEndDate { get; set; }
        public string? ClosedDate { get; set; }
        public string? ClosedBy { get; set; }
        public string? ClosingNotes { get; set; }
        public string Status { get; set; } = "ABIERTO";
        public decimal QuotedAmount { get; set; }
        public decimal InvoicedAmount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal NetProfit => InvoicedAmount > 0 ? (InvoicedAmount - TotalCost) : (QuotedAmount - TotalCost);
        public decimal MarginPercent => (InvoicedAmount > 0 ? InvoicedAmount : QuotedAmount) > 0 
            ? (NetProfit / (InvoicedAmount > 0 ? InvoicedAmount : QuotedAmount)) * 100 
            : 0;
        public List<ProyectoCostoItemDto> Items { get; set; } = new();
    }
}
