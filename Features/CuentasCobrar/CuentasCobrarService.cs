using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.CuentasCobrar.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.CuentasCobrar
{
    public class CuentasCobrarService
    {
        private readonly SmartHnlDbContext _context;

        public CuentasCobrarService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<CuentaPorCobrarDto>> GetAllAsync()
        {
            return await _context.CuentasPorCobrar
                .AsNoTracking()
                .Include(c => c.Client)
                .OrderByDescending(c => c.DueDate)
                .Select(c => new CuentaPorCobrarDto
                {
                    Id = c.Id,
                    InvoiceId = c.InvoiceId,
                    InvoiceNumber = c.InvoiceNumber,
                    ClientId = c.ClientId,
                    ClientName = c.Client != null ? c.Client.Name : null,
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
            var cxc = await _context.CuentasPorCobrar.FindAsync(id);
            if (cxc == null || cxc.Balance <= 0) return false;

            decimal newPaid = cxc.PaidAmount + dto.Amount;
            decimal newBalance = Math.Max(0, cxc.TotalAmount - newPaid);

            cxc.PaidAmount = newPaid;
            cxc.Balance = newBalance;
            cxc.Status = newBalance == 0 ? "PAGADA" : "PARCIAL";

            var paymentRecord = new
            {
                id = Guid.NewGuid().ToString(),
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                amount = dto.Amount,
                method = dto.PaymentMethod,
                reference = dto.Reference ?? "",
                notes = dto.Notes ?? ""
            };

            var list = string.IsNullOrEmpty(cxc.Payments) || cxc.Payments == "[]"
                ? new List<object>()
                : JsonSerializer.Deserialize<List<object>>(cxc.Payments) ?? new List<object>();

            list.Add(paymentRecord);
            cxc.Payments = JsonSerializer.Serialize(list);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
