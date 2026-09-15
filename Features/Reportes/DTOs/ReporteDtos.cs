using System.Collections.Generic;

namespace SmartHnl.API.Features.Reportes.DTOs
{
    public class ReporteUtilidadItemDto
    {
        public string InvoiceNumber { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string DocumentType { get; set; } = null!;
        public string ClientName { get; set; } = "";
        public decimal TotalCost { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Isv { get; set; }
        public decimal Total { get; set; }
        public decimal NetProfit { get; set; }
        public decimal MarginPercent { get; set; }
    }

    public class ReporteUtilidadConsolidadoDto
    {
        public decimal TotalCost { get; set; }
        public decimal TotalSubtotal { get; set; }
        public decimal TotalIsv { get; set; }
        public decimal TotalGeneral { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal GlobalMarginPercent => TotalGeneral > 0 ? (TotalProfit / TotalGeneral) * 100 : 0;
        public List<ReporteUtilidadItemDto> Items { get; set; } = new();
    }

    public class ArqueoCajaItemDto
    {
        public string PaymentTerm { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public int InvoicesCount { get; set; }
    }
}
