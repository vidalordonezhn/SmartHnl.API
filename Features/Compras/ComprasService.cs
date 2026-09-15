using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.Compras.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Compras
{
    public class ComprasService
    {
        private readonly SmartHnlDbContext _context;

        public ComprasService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<CompraDto>> GetAllAsync()
        {
            return await _context.Compras
                .AsNoTracking()
                .Include(c => c.Provider)
                .Include(c => c.Details)
                    .ThenInclude(d => d.Product)
                .OrderByDescending(c => c.Date)
                .Select(c => new CompraDto
                {
                    Id = c.Id,
                    PurchaseNumber = c.PurchaseNumber,
                    Date = c.Date,
                    ProviderId = c.ProviderId,
                    ProviderName = c.Provider != null ? c.Provider.Name : null,
                    ProviderRtn = c.Provider != null ? c.Provider.Rtn : null,
                    PaymentType = c.PaymentType,
                    PaymentTerm = c.PaymentTerm,
                    TotalGeneral = c.TotalGeneral,
                    Comments = c.Comments,
                    Status = c.Status,
                    Details = c.Details.Select(d => new CompraDetalleDto
                    {
                        Id = d.Id,
                        ProductId = d.ProductId,
                        ProductName = d.Product != null ? d.Product.Name : null,
                        Quantity = d.Quantity,
                        CostUnit = d.CostUnit
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<CompraDto> CreateAsync(CompraCreateDto dto)
        {
            decimal total = dto.Details.Sum(d => d.Quantity * d.CostUnit);

            var compra = new Compra
            {
                PurchaseNumber = dto.PurchaseNumber.Trim(),
                Date = dto.Date ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ProviderId = dto.ProviderId,
                PaymentType = dto.PaymentType,
                PaymentTerm = dto.PaymentTerm,
                TotalGeneral = total,
                Comments = dto.Comments,
                Status = "RECIBIDA"
            };

            foreach (var item in dto.Details)
            {
                compra.Details.Add(new CompraDetalle
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    CostUnit = item.CostUnit
                });

                // Aumentar stock y registrar en Kardex
                var prod = await _context.Productos.FindAsync(item.ProductId);
                if (prod != null)
                {
                    prod.Stock += item.Quantity;
                    prod.PurchasePrice = item.CostUnit; // Actualizar último costo

                    var kardex = new Kardex
                    {
                        ProductId = prod.Id,
                        Date = compra.Date,
                        Type = "COMPRA",
                        Quantity = item.Quantity,
                        CostUnit = item.CostUnit,
                        StockAfter = prod.Stock,
                        Reference = compra.PurchaseNumber,
                        Notes = $"Compra Fac. {compra.PurchaseNumber}"
                    };
                    await _context.KardexEntries.AddAsync(kardex);
                }
            }

            // Si es a crédito, registrar en Cuentas por Pagar (CxP)
            if (dto.PaymentType == "CREDITO")
            {
                var cxp = new CuentaPorPagar
                {
                    PurchaseId = compra.Id,
                    PurchaseNumber = compra.PurchaseNumber,
                    ProviderId = compra.ProviderId,
                    TotalAmount = total,
                    PaidAmount = 0.00m,
                    Balance = total,
                    Status = "PENDIENTE",
                    DueDate = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd"),
                    Payments = "[]"
                };
                await _context.CuentasPorPagar.AddAsync(cxp);
            }

            await _context.Compras.AddAsync(compra);
            await _context.SaveChangesAsync();

            return (await GetAllAsync()).First(c => c.Id == compra.Id);
        }
    }
}
