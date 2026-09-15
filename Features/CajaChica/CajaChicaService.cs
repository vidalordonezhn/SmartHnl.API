using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.CajaChica.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.CajaChica
{
    public class CajaChicaService
    {
        private readonly SmartHnlDbContext _context;

        public CajaChicaService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<CajaChicaDto>> GetCajasAsync()
        {
            var cajas = await _context.CajasChicas.AsNoTracking().ToListAsync();
            var result = new List<CajaChicaDto>();

            foreach (var c in cajas)
            {
                var ingresos = await _context.CajaChicaMovimientos
                    .Where(m => m.PettyCashId == c.Id && (m.MovementType == "INGRESO" || m.MovementType == "APERTURA" || m.MovementType == "REEMBOLSO"))
                    .SumAsync(m => (decimal?)m.Amount) ?? 0;

                var egresos = await _context.CajaChicaMovimientos
                    .Where(m => m.PettyCashId == c.Id && m.MovementType == "EGRESO")
                    .SumAsync(m => (decimal?)m.Amount) ?? 0;

                result.Add(new CajaChicaDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    BaseAmount = c.BaseAmount,
                    MinLimit = c.MinLimit,
                    Responsible = c.Responsible,
                    UserId = c.UserId,
                    FundType = c.FundType,
                    BankId = c.BankId,
                    BankName = c.BankName,
                    BankAccountNumber = c.BankAccountNumber,
                    Status = c.Status,
                    Notes = c.Notes,
                    CurrentBalance = (c.BaseAmount + ingresos) - egresos
                });
            }

            return result;
        }

        public async Task<List<CajaChicaGastoDto>> GetGastosAsync(string? pettyCashId = null)
        {
            var query = _context.CajaChicaGastos
                .AsNoTracking()
                .Include(g => g.Account)
                .Include(g => g.Creditor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(pettyCashId))
            {
                query = query.Where(g => g.PettyCashId == pettyCashId);
            }

            return await query
                .OrderByDescending(g => g.ExpenseDate)
                .Select(g => new CajaChicaGastoDto
                {
                    Id = g.Id,
                    PettyCashId = g.PettyCashId,
                    AccountId = g.AccountId,
                    AccountCode = g.Account != null ? g.Account.Code : null,
                    AccountName = g.Account != null ? g.Account.Name : null,
                    CreditorId = g.CreditorId,
                    CreditorName = g.Creditor != null ? g.Creditor.Name : null,
                    ExpenseDate = g.ExpenseDate,
                    Description = g.Description,
                    ReceiptNumber = g.ReceiptNumber,
                    Amount = g.Amount,
                    Status = g.Status,
                    Notes = g.Notes
                }).ToListAsync();
        }

        public async Task<CajaChicaGastoDto> CreateGastoAsync(CajaChicaGastoDto dto)
        {
            var gasto = new CajaChicaGasto
            {
                PettyCashId = dto.PettyCashId,
                AccountId = dto.AccountId,
                CreditorId = dto.CreditorId,
                ExpenseDate = dto.ExpenseDate ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Description = dto.Description,
                ReceiptNumber = dto.ReceiptNumber,
                Amount = dto.Amount,
                Status = "PENDING",
                Notes = dto.Notes
            };

            var mov = new CajaChicaMovimiento
            {
                PettyCashId = dto.PettyCashId,
                MovementType = "EGRESO",
                Amount = dto.Amount,
                MovementDate = gasto.ExpenseDate,
                ReferenceType = "GASTO",
                ReferenceId = gasto.Id,
                Description = gasto.Description
            };

            await _context.CajaChicaGastos.AddAsync(gasto);
            await _context.CajaChicaMovimientos.AddAsync(mov);
            await _context.SaveChangesAsync();

            dto.Id = gasto.Id;
            return dto;
        }

        public async Task<List<CajaChicaDocumentoDto>> GetDocumentosAsync()
        {
            return await _context.CajaChicaDocumentos
                .AsNoTracking()
                .Include(d => d.PettyCash)
                .Include(d => d.Items)
                    .ThenInclude(i => i.Expense)
                        .ThenInclude(e => e!.Account)
                .OrderByDescending(d => d.DocumentDate)
                .Select(d => new CajaChicaDocumentoDto
                {
                    Id = d.Id,
                    DocumentNumber = d.DocumentNumber,
                    PettyCashId = d.PettyCashId,
                    PettyCashName = d.PettyCash != null ? d.PettyCash.Name : null,
                    DocumentDate = d.DocumentDate,
                    Responsible = d.Responsible,
                    Status = d.Status,
                    Notes = d.Notes,
                    TotalAmount = d.Items.Sum(i => i.Expense != null ? i.Expense.Amount : 0),
                    Expenses = d.Items.Where(i => i.Expense != null).Select(i => new CajaChicaGastoDto
                    {
                        Id = i.Expense!.Id,
                        PettyCashId = i.Expense.PettyCashId,
                        AccountId = i.Expense.AccountId,
                        AccountCode = i.Expense.Account != null ? i.Expense.Account.Code : null,
                        AccountName = i.Expense.Account != null ? i.Expense.Account.Name : null,
                        ExpenseDate = i.Expense.ExpenseDate,
                        Description = i.Expense.Description,
                        ReceiptNumber = i.Expense.ReceiptNumber,
                        Amount = i.Expense.Amount,
                        Status = i.Expense.Status
                    }).ToList()
                }).ToListAsync();
        }
    }
}
