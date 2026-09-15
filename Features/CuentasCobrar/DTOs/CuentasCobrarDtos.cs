namespace SmartHnl.API.Features.CuentasCobrar.DTOs
{
    public class CuentaPorCobrarDto
    {
        public string Id { get; set; } = null!;
        public string InvoiceId { get; set; } = null!;
        public string InvoiceNumber { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string? ClientName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; } = "PENDIENTE";
        public string DueDate { get; set; } = null!;
        public string Payments { get; set; } = "[]";
    }

    public class RegistrarAbonoDto
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "EFECTIVO";
        public string? Reference { get; set; }
        public string? Notes { get; set; }
    }
}
