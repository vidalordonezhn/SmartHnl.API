namespace SmartHnl.API.Features.CuentasPagar.DTOs
{
    public class CuentaPorPagarDto
    {
        public string Id { get; set; } = null!;
        public string PurchaseId { get; set; } = null!;
        public string PurchaseNumber { get; set; } = null!;
        public string ProviderId { get; set; } = null!;
        public string? ProviderName { get; set; }
        public string? ProviderRtn { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; } = "PENDIENTE";
        public string DueDate { get; set; } = null!;
        public string Payments { get; set; } = "[]";
    }

    public class RegistrarPagoProveedorDto
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "TRANSFERENCIA";
        public string? Reference { get; set; }
        public string? Notes { get; set; }
    }
}
