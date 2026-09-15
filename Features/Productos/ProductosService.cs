using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.Productos.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Productos
{
    public class ProductosService
    {
        private readonly SmartHnlDbContext _context;

        public ProductosService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductoDto>> GetAllAsync()
        {
            return await _context.Productos
                .AsNoTracking()
                .Include(p => p.Category)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    ProductCode = p.ProductCode,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : null,
                    PurchasePrice = p.PurchasePrice,
                    SellPrice = p.SellPrice,
                    MarginPercent = p.MarginPercent,
                    TaxType = p.TaxType,
                    Stock = p.Stock,
                    IsService = p.IsService,
                    Barcode = p.Barcode,
                    FastCode = p.FastCode,
                    Brand = p.Brand,
                    UnitOfMeasure = p.UnitOfMeasure,
                    LocationBodega = p.LocationBodega,
                    StockMin = p.StockMin,
                    PricesJson = p.PricesJson,
                    ImageUrl = p.ImageUrl,
                    RoundingType = p.RoundingType,
                    ManageSeries = p.ManageSeries,
                    TieneGarantia = p.TieneGarantia,
                    DiasGarantia = p.DiasGarantia
                }).ToListAsync();
        }

        public async Task<ProductoDto?> GetByIdAsync(string id)
        {
            return await _context.Productos
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.Id == id)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    ProductCode = p.ProductCode,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : null,
                    PurchasePrice = p.PurchasePrice,
                    SellPrice = p.SellPrice,
                    MarginPercent = p.MarginPercent,
                    TaxType = p.TaxType,
                    Stock = p.Stock,
                    IsService = p.IsService,
                    Barcode = p.Barcode,
                    FastCode = p.FastCode,
                    Brand = p.Brand,
                    UnitOfMeasure = p.UnitOfMeasure,
                    LocationBodega = p.LocationBodega,
                    StockMin = p.StockMin,
                    PricesJson = p.PricesJson,
                    ImageUrl = p.ImageUrl,
                    RoundingType = p.RoundingType,
                    ManageSeries = p.ManageSeries,
                    TieneGarantia = p.TieneGarantia,
                    DiasGarantia = p.DiasGarantia
                }).FirstOrDefaultAsync();
        }

        public async Task<ProductoDto> CreateAsync(ProductoCreateDto dto)
        {
            var p = new Producto
            {
                ProductCode = dto.ProductCode,
                Name = dto.Name.Trim(),
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                PurchasePrice = dto.PurchasePrice,
                SellPrice = dto.SellPrice,
                MarginPercent = dto.MarginPercent,
                TaxType = dto.TaxType,
                Stock = dto.Stock,
                IsService = dto.IsService,
                Barcode = dto.Barcode,
                FastCode = dto.FastCode,
                Brand = dto.Brand,
                UnitOfMeasure = dto.UnitOfMeasure,
                LocationBodega = dto.LocationBodega,
                StockMin = dto.StockMin,
                PricesJson = dto.PricesJson,
                ImageUrl = dto.ImageUrl,
                RoundingType = dto.RoundingType,
                ManageSeries = dto.ManageSeries,
                TieneGarantia = dto.TieneGarantia,
                DiasGarantia = dto.DiasGarantia
            };

            await _context.Productos.AddAsync(p);

            if (p.Stock > 0 && p.IsService == 0)
            {
                var kardex = new Kardex
                {
                    ProductId = p.Id,
                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Type = "ENTRADA",
                    Quantity = p.Stock,
                    CostUnit = p.PurchasePrice,
                    StockAfter = p.Stock,
                    Reference = "INVENTARIO INICIAL",
                    Notes = "Apertura o creación de producto"
                };
                await _context.KardexEntries.AddAsync(kardex);
            }

            await _context.SaveChangesAsync();
            return (await GetByIdAsync(p.Id))!;
        }

        public async Task<ProductoDto?> UpdateAsync(string id, ProductoCreateDto dto)
        {
            var p = await _context.Productos.FindAsync(id);
            if (p == null) return null;

            p.ProductCode = dto.ProductCode;
            p.Name = dto.Name.Trim();
            p.Description = dto.Description;
            p.CategoryId = dto.CategoryId;
            p.PurchasePrice = dto.PurchasePrice;
            p.SellPrice = dto.SellPrice;
            p.MarginPercent = dto.MarginPercent;
            p.TaxType = dto.TaxType;
            p.Stock = dto.Stock;
            p.IsService = dto.IsService;
            p.Barcode = dto.Barcode;
            p.FastCode = dto.FastCode;
            p.Brand = dto.Brand;
            p.UnitOfMeasure = dto.UnitOfMeasure;
            p.LocationBodega = dto.LocationBodega;
            p.StockMin = dto.StockMin;
            p.PricesJson = dto.PricesJson;
            p.ImageUrl = dto.ImageUrl;
            p.RoundingType = dto.RoundingType;
            p.ManageSeries = dto.ManageSeries;
            p.TieneGarantia = dto.TieneGarantia;
            p.DiasGarantia = dto.DiasGarantia;

            await _context.SaveChangesAsync();
            return (await GetByIdAsync(id))!;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var p = await _context.Productos.FindAsync(id);
            if (p == null) return false;
            _context.Productos.Remove(p);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
