using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.CuentasCobrar.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.CuentasPagar
{
    public class CuentasPagarService
    {
        private readonly SmartHnlDbContext _context;

        public CuentasPagarService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<CuentaPorCobrarDto>> GetAllAsync()
        {
            return await _context.CuentasPorPagar
                .AsNoTracking()
                .OrderByDescending(c => c.DueDate)
                .Select(c => new CuentaPorCobrarDto
                {
                    Id = c.Id,
                    InvoiceId = c.PurchaseId,
                    InvoiceNumber = c.PurchaseNumber,
                    ClientId = c.ProviderId,
                    TotalAmount = c.TotalAmount,
                    PaidAmount = c.PaidAmount,
                    Balance = c.Balance,
                    Status = c.Status,
                    DueDate = c.DueDate,
                    Payments = c.Payments
                }).ToListAsync();
        }

        public async Task<bool> AddPaymentAsync(string id, RegistrarAbonoDto dto)
        {
            var cxp = await _context.CuentasPorPagar.FindAsync(id);
            if (cxp == null || cxp.Balance <= 0) return false;

            decimal newPaid = cxp.PaidAmount + dto.Amount;
            decimal newBalance = Math.Max(0, cxp.TotalAmount - newPaid);

            cxp.PaidAmount = newPaid;
            cxp.Balance = newBalance;
            cxp.Status = newBalance == 0 ? "PAGADA" : "PARCIAL";

            var paymentRecord = new
            {
                id = Guid.NewGuid().ToString(),
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                amount = dto.Amount,
                method = dto.PaymentMethod,
                reference = dto.Reference ?? "",
                notes = dto.Notes ?? ""
            };

            var list = string.IsNullOrEmpty(cxp.Payments) || cxp.Payments == "[]"
                ? new List<object>()
                : JsonSerializer.Deserialize<List<object>>(cxp.Payments) ?? new List<object>();

            list.Add(paymentRecord);
            cxp.Payments = JsonSerializer.Serialize(list);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
