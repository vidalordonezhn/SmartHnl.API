using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.Inventario.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Inventario
{
    public class InventarioService
    {
        private readonly SmartHnlDbContext _context;

        public InventarioService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<KardexDto>> GetKardexAsync(string? productId = null)
        {
            var query = _context.KardexEntries
                .AsNoTracking()
                .Include(k => k.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(productId))
            {
                query = query.Where(k => k.ProductId == productId);
            }

            return await query
                .OrderByDescending(k => k.Date)
                .Select(k => new KardexDto
                {
                    Id = k.Id,
                    ProductId = k.ProductId,
                    ProductName = k.Product != null ? k.Product.Name : null,
                    Date = k.Date,
                    Type = k.Type,
                    Quantity = k.Quantity,
                    CostUnit = k.CostUnit,
                    StockAfter = k.StockAfter,
                    Reference = k.Reference,
                    Notes = k.Notes
                }).ToListAsync();
        }

        public async Task<bool> AdjustInventoryAsync(InventoryAdjustmentDto dto)
        {
            var product = await _context.Productos.FindAsync(dto.ProductId);
            if (product == null) return false;

            var adjType = dto.Type?.ToUpper() == "ENTRADA" ? "ENTRADA" : "SALIDA";

            decimal newStock = adjType == "ENTRADA" 
                ? product.Stock + dto.Quantity 
                : product.Stock - dto.Quantity;

            if (newStock < 0) newStock = 0;
            product.Stock = newStock;

            var unitCost = dto.CostUnit ?? product.PurchasePrice;
            var refStr = !string.IsNullOrWhiteSpace(dto.Reference) ? dto.Reference.Trim() : $"AJUSTE-{DateTime.Now:yyyyMMddHHmmss}";
            var notesStr = !string.IsNullOrWhiteSpace(dto.Notes) ? dto.Notes.Trim() : (!string.IsNullOrWhiteSpace(dto.Reason) ? dto.Reason.Trim() : $"Ajuste manual de {adjType.ToLower()}");

            var kardex = new Kardex
            {
                ProductId = product.Id,
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Type = adjType,
                Quantity = dto.Quantity,
                CostUnit = unitCost,
                StockAfter = newStock,
                Reference = refStr,
                Notes = notesStr
            };

            await _context.KardexEntries.AddAsync(kardex);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RecalculateInventoryAsync()
        {
            var products = await _context.Productos.ToListAsync();
            foreach (var p in products)
            {
                var entries = await _context.KardexEntries
                    .Where(k => k.ProductId == p.Id)
                    .OrderBy(k => k.Date)
                    .ToListAsync();

                decimal runningStock = 0;
                foreach (var k in entries)
                {
                    if (k.Type is "ENTRADA" or "COMPRA" or "AJUSTE")
                    {
                        runningStock += k.Quantity;
                    }
                    else if (k.Type is "SALIDA" or "VENTA")
                    {
                        runningStock -= k.Quantity;
                    }
                    k.StockAfter = runningStock;
                }

                p.Stock = Math.Max(0, runningStock);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // Series
        public async Task<List<SerieDto>> GetSeriesAsync(string? productId = null)
        {
            var query = _context.Series.AsNoTracking().Include(s => s.Product).AsQueryable();
            if (!string.IsNullOrEmpty(productId))
            {
                query = query.Where(s => s.ProductId == productId);
            }

            return await query.Select(s => new SerieDto
            {
                Id = s.Id,
                ProductId = s.ProductId,
                ProductName = s.Product != null ? s.Product.Name : null,
                NumeroSerie = s.NumeroSerie,
                Estado = s.Estado,
                InvoiceId = s.InvoiceId,
                ClientId = s.ClientId,
                PurchaseId = s.PurchaseId,
                ProviderId = s.ProviderId,
                CostoCompra = s.CostoCompra,
                FechaIngreso = s.FechaIngreso,
                FechaVenta = s.FechaVenta,
                UsuarioVenta = s.UsuarioVenta,
                Notas = s.Notas
            }).ToListAsync();
        }

        public async Task<SerieDto> CreateSerieAsync(SerieDto dto)
        {
            var s = new Serie
            {
                ProductId = dto.ProductId,
                NumeroSerie = dto.NumeroSerie,
                Estado = dto.Estado ?? "Disponible",
                CostoCompra = dto.CostoCompra,
                FechaIngreso = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Notas = dto.Notas
            };
            await _context.Series.AddAsync(s);
            await _context.SaveChangesAsync();
            dto.Id = s.Id;
            return dto;
        }
    }
}
