using System.Collections.Generic;

namespace SmartHnl.API.Features.CajaChica.DTOs
{
    public class CajaChicaDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal BaseAmount { get; set; }
        public decimal MinLimit { get; set; }
        public string Responsible { get; set; } = null!;
        public string? UserId { get; set; }
        public string FundType { get; set; } = "CHECK";
        public string? BankId { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }
        public string Status { get; set; } = "ACTIVE";
        public string? Notes { get; set; }
        public decimal CurrentBalance { get; set; }
    }

    public class CajaChicaGastoDto
    {
        public string Id { get; set; } = null!;
        public string PettyCashId { get; set; } = null!;
        public string AccountId { get; set; } = null!;
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public string? CreditorId { get; set; }
        public string? CreditorName { get; set; }
        public string ExpenseDate { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ReceiptNumber { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "PENDING";
        public string? Notes { get; set; }
    }

    public class CajaChicaDocumentoDto
    {
        public string Id { get; set; } = null!;
        public string DocumentNumber { get; set; } = null!;
        public string PettyCashId { get; set; } = null!;
        public string? PettyCashName { get; set; }
        public string DocumentDate { get; set; } = null!;
        public string Responsible { get; set; } = null!;
        public string Status { get; set; } = "PENDING_REIMBURSEMENT";
        public string? Notes { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CajaChicaGastoDto> Expenses { get; set; } = new();
    }
}
