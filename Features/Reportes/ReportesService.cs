using SmartHnl.API.Data;
using SmartHnl.API.Features.Reportes.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Reportes
{
    public class ReportesService
    {
        private readonly SmartHnlDbContext _context;

        public ReportesService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<ReporteUtilidadConsolidadoDto> GetReporteUtilidadConsolidadoAsync(string? establishment = "ALL", string? startDate = null, string? endDate = null)
        {
            var query = _context.Facturas
                .AsNoTracking()
                .Include(f => f.Client)
                .Include(f => f.Details)
                .Where(f => f.Status == "EMITIDA")
                .AsQueryable();

            if (!string.IsNullOrEmpty(establishment) && establishment != "ALL")
            {
                query = query.Where(f => f.DocumentType == establishment);
            }

            if (!string.IsNullOrEmpty(startDate)) query = query.Where(f => f.Date.CompareTo(startDate) >= 0);
            if (!string.IsNullOrEmpty(endDate)) query = query.Where(f => f.Date.CompareTo(endDate) <= 0);

            var facturas = await query.ToListAsync();

            var result = new ReporteUtilidadConsolidadoDto();

            foreach (var f in facturas)
            {
                decimal cost = f.Details.Sum(d => d.Quantity * d.CostUnit);
                decimal subtotal = f.SubtotalGravado + f.SubtotalExento + f.SubtotalExonerado;
                decimal profit = subtotal - cost;

                result.TotalCost += cost;
                result.TotalSubtotal += subtotal;
                result.TotalIsv += f.IsvTotal;
                result.TotalGeneral += f.TotalGeneral;
                result.TotalProfit += profit;

                result.Items.Add(new ReporteUtilidadItemDto
                {
                    InvoiceNumber = f.InvoiceNumber,
                    Date = f.Date,
                    DocumentType = f.DocumentType,
                    ClientName = f.CustomClientName ?? (f.Client != null ? f.Client.Name : "Cliente"),
                    TotalCost = cost,
                    Subtotal = subtotal,
                    Isv = f.IsvTotal,
                    Total = f.TotalGeneral,
                    NetProfit = profit,
                    MarginPercent = subtotal > 0 ? (profit / subtotal) * 100 : 0
                });
            }

            return result;
        }

        public async Task<List<ArqueoCajaItemDto>> GetArqueoDiarioAsync(string date, string? establishment = "ALL")
        {
            var query = _context.Facturas
                .AsNoTracking()
                .Where(f => f.Status == "EMITIDA" && f.Date.StartsWith(date))
                .AsQueryable();

            if (!string.IsNullOrEmpty(establishment) && establishment != "ALL")
            {
                query = query.Where(f => f.DocumentType == establishment);
            }

            return await query
                .GroupBy(f => f.PaymentTerm ?? f.PaymentType)
                .Select(g => new ArqueoCajaItemDto
                {
                    PaymentTerm = g.Key,
                    TotalAmount = g.Sum(f => f.TotalGeneral),
                    InvoicesCount = g.Count()
                }).ToListAsync();
        }
    }
}
