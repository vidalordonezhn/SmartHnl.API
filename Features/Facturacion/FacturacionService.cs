using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.Facturacion.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Facturacion
{
    public class FacturacionService
    {
        private readonly SmartHnlDbContext _context;

        public FacturacionService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<FacturaDto>> GetAllAsync(string? establishment = "ALL")
        {
            var query = _context.Facturas
                .AsNoTracking()
                .Include(f => f.Client)
                .Include(f => f.User)
                .Include(f => f.Details)
                    .ThenInclude(d => d.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(establishment) && establishment != "ALL")
            {
                query = query.Where(f => f.DocumentType == establishment);
            }

            return await query
                .OrderByDescending(f => f.Date)
                .Select(f => new FacturaDto
                {
                    Id = f.Id,
                    InvoiceNumber = f.InvoiceNumber,
                    DocumentType = f.DocumentType,
                    Date = f.Date,
                    ClientId = f.ClientId,
                    ClientName = f.CustomClientName ?? (f.Client != null ? f.Client.Name : null),
                    ClientRtn = f.CustomClientRtn ?? (f.Client != null ? f.Client.Rtn : null),
                    Status = f.Status,
                    PaymentType = f.PaymentType,
                    PaymentTerm = f.PaymentTerm,
                    DueDate = f.DueDate,
                    IsExonerated = f.IsExonerated,
                    OrdenCompraExenta = f.OrdenCompraExenta,
                    ConstanciaExoneracion = f.ConstanciaExoneracion,
                    DocumentoSar = f.DocumentoSar,
                    SubtotalGravado = f.SubtotalGravado,
                    SubtotalExento = f.SubtotalExento,
                    SubtotalExonerado = f.SubtotalExonerado,
                    IsvTotal = f.IsvTotal,
                    TotalGeneral = f.TotalGeneral,
                    Comments = f.Comments,
                    UserId = f.UserId,
                    UserName = f.User != null ? f.User.Name : null,
                    CustomClientName = f.CustomClientName,
                    CustomClientRtn = f.CustomClientRtn,
                    CustomClientAddress = f.CustomClientAddress,
                    CustomClientPhone = f.CustomClientPhone,
                    CustomClientEmail = f.CustomClientEmail,
                    Recalcular = f.Recalcular,
                    Details = f.Details.Select(d => new FacturaDetalleDto
                    {
                        Id = d.Id,
                        ProductId = d.ProductId,
                        ProductName = d.Product != null ? d.Product.Name : null,
                        Quantity = d.Quantity,
                        PriceUnit = d.PriceUnit,
                        TaxType = d.TaxType,
                        MontoGravado = d.MontoGravado,
                        MontoExento = d.MontoExento,
                        MontoExonerado = d.MontoExonerado,
                        IsvCalculated = d.IsvCalculated,
                        SerialNumber = d.SerialNumber,
                        CostUnit = d.CostUnit,
                        CustomDescription = d.CustomDescription,
                        Discount = d.Discount
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<FacturaDto?> GetByIdAsync(string id)
        {
            return await _context.Facturas
                .AsNoTracking()
                .Include(f => f.Client)
                .Include(f => f.User)
                .Include(f => f.Details)
                    .ThenInclude(d => d.Product)
                .Where(f => f.Id == id)
                .Select(f => new FacturaDto
                {
                    Id = f.Id,
                    InvoiceNumber = f.InvoiceNumber,
                    DocumentType = f.DocumentType,
                    Date = f.Date,
                    ClientId = f.ClientId,
                    ClientName = f.CustomClientName ?? (f.Client != null ? f.Client.Name : null),
                    ClientRtn = f.CustomClientRtn ?? (f.Client != null ? f.Client.Rtn : null),
                    Status = f.Status,
                    PaymentType = f.PaymentType,
                    PaymentTerm = f.PaymentTerm,
                    DueDate = f.DueDate,
                    IsExonerated = f.IsExonerated,
                    SubtotalGravado = f.SubtotalGravado,
                    SubtotalExento = f.SubtotalExento,
                    SubtotalExonerado = f.SubtotalExonerado,
                    IsvTotal = f.IsvTotal,
                    TotalGeneral = f.TotalGeneral,
                    Comments = f.Comments,
                    UserId = f.UserId,
                    UserName = f.User != null ? f.User.Name : null,
                    CustomClientName = f.CustomClientName,
                    CustomClientRtn = f.CustomClientRtn,
                    CustomClientAddress = f.CustomClientAddress,
                    CustomClientPhone = f.CustomClientPhone,
                    CustomClientEmail = f.CustomClientEmail,
                    Recalcular = f.Recalcular,
                    Details = f.Details.Select(d => new FacturaDetalleDto
                    {
                        Id = d.Id,
                        ProductId = d.ProductId,
                        ProductName = d.Product != null ? d.Product.Name : null,
                        Quantity = d.Quantity,
                        PriceUnit = d.PriceUnit,
                        TaxType = d.TaxType,
                        MontoGravado = d.MontoGravado,
                        MontoExento = d.MontoExento,
                        MontoExonerado = d.MontoExonerado,
                        IsvCalculated = d.IsvCalculated,
                        SerialNumber = d.SerialNumber,
                        CostUnit = d.CostUnit,
                        CustomDescription = d.CustomDescription,
                        Discount = d.Discount
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<FacturaDto> CreateInvoiceAsync(FacturaCreateDto dto, string userId)
        {
            var config = await _context.Configuracion.FirstOrDefaultAsync(c => c.Id == 1)
                ?? throw new Exception("Configuración fiscal no encontrada.");

            string nextInvoiceNum;
            bool isSucursal = dto.DocumentType == "RECIBO_HONORARIOS";

            if (isSucursal)
            {
                nextInvoiceNum = IncrementCorrelative(config.HonorariosCurrentNumber ?? "000-002-01-00000000");
                config.HonorariosCurrentNumber = nextInvoiceNum;
            }
            else
            {
                nextInvoiceNum = IncrementCorrelative(config.CurrentInvoiceNumber);
                config.CurrentInvoiceNumber = nextInvoiceNum;
            }

            decimal subtotalGravado = 0;
            decimal subtotalExento = 0;
            decimal subtotalExonerado = 0;
            decimal isvTotal = 0;

            var factura = new Factura
            {
                InvoiceNumber = nextInvoiceNum,
                DocumentType = dto.DocumentType,
                Date = dto.Date ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ClientId = dto.ClientId,
                Status = "EMITIDA",
                PaymentType = dto.PaymentType,
                PaymentTerm = dto.PaymentTerm,
                DueDate = dto.DueDate ?? DateTime.Now.ToString("yyyy-MM-dd"),
                IsExonerated = dto.IsExonerated,
                OrdenCompraExenta = dto.OrdenCompraExenta,
                ConstanciaExoneracion = dto.ConstanciaExoneracion,
                DocumentoSar = dto.DocumentoSar,
                Comments = dto.Comments,
                UserId = userId,
                CustomClientName = dto.CustomClientName,
                CustomClientRtn = dto.CustomClientRtn,
                CustomClientAddress = dto.CustomClientAddress,
                CustomClientPhone = dto.CustomClientPhone,
                CustomClientEmail = dto.CustomClientEmail,
                Recalcular = dto.Recalcular
            };

            foreach (var item in dto.Details)
            {
                var product = await _context.Productos.FindAsync(item.ProductId);
                decimal cost = product?.PurchasePrice ?? item.CostUnit;
                decimal gross = (item.Quantity * item.PriceUnit) - item.Discount;

                decimal mGravado = 0, mExento = 0, mExonerado = 0, isv = 0;

                if (dto.IsExonerated == 1)
                {
                    mExonerado = gross;
                }
                else if (item.TaxType == "EXENTO")
                {
                    mExento = gross;
                }
                else if (item.TaxType == "GRAVADO_18")
                {
                    mGravado = gross;
                    isv = Math.Round(gross * 0.18m, 2);
                }
                else
                {
                    mGravado = gross;
                    isv = Math.Round(gross * 0.15m, 2);
                }

                subtotalGravado += mGravado;
                subtotalExento += mExento;
                subtotalExonerado += mExonerado;
                isvTotal += isv;

                factura.Details.Add(new FacturaDetalle
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    PriceUnit = item.PriceUnit,
                    TaxType = item.TaxType,
                    MontoGravado = mGravado,
                    MontoExento = mExento,
                    MontoExonerado = mExonerado,
                    IsvCalculated = isv,
                    SerialNumber = item.SerialNumber,
                    CostUnit = cost,
                    CustomDescription = item.CustomDescription,
                    Discount = item.Discount
                });

                // Descontar inventario y registrar en Kardex si es producto físico
                if (product != null && product.IsService == 0)
                {
                    product.Stock = Math.Max(0, product.Stock - item.Quantity);

                    var kardex = new Kardex
                    {
                        ProductId = product.Id,
                        Date = factura.Date,
                        Type = "VENTA",
                        Quantity = item.Quantity,
                        CostUnit = cost,
                        StockAfter = product.Stock,
                        Reference = nextInvoiceNum,
                        Notes = $"Venta Factura {nextInvoiceNum}"
                    };
                    await _context.KardexEntries.AddAsync(kardex);
                }
            }

            factura.SubtotalGravado = subtotalGravado;
            factura.SubtotalExento = subtotalExento;
            factura.SubtotalExonerado = subtotalExonerado;
            factura.IsvTotal = isvTotal;
            factura.TotalGeneral = subtotalGravado + subtotalExento + subtotalExonerado + isvTotal;

            // Si es a crédito, registrar en Cuentas por Cobrar
            if (dto.PaymentType == "CREDITO")
            {
                var cxc = new CuentaPorCobrar
                {
                    InvoiceId = factura.Id,
                    InvoiceNumber = factura.InvoiceNumber,
                    ClientId = factura.ClientId,
                    TotalAmount = factura.TotalGeneral,
                    PaidAmount = 0.00m,
                    Balance = factura.TotalGeneral,
                    Status = "PENDIENTE",
                    DueDate = factura.DueDate ?? DateTime.Now.AddDays(30).ToString("yyyy-MM-dd"),
                    Payments = "[]"
                };
                await _context.CuentasPorCobrar.AddAsync(cxc);
            }

            await _context.Facturas.AddAsync(factura);
            await _context.SaveChangesAsync();

            return (await GetByIdAsync(factura.Id))!;
        }

        public async Task<bool> AnnulInvoiceAsync(string id)
        {
            var factura = await _context.Facturas
                .Include(f => f.Details)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factura == null || factura.Status == "ANULADA") return false;

            factura.Status = "ANULADA";

            // Revertir inventario
            foreach (var d in factura.Details)
            {
                var prod = await _context.Productos.FindAsync(d.ProductId);
                if (prod != null && prod.IsService == 0)
                {
                    prod.Stock += d.Quantity;
                    var kardex = new Kardex
                    {
                        ProductId = prod.Id,
                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        Type = "ENTRADA",
                        Quantity = d.Quantity,
                        CostUnit = d.CostUnit,
                        StockAfter = prod.Stock,
                        Reference = $"ANULACION {factura.InvoiceNumber}",
                        Notes = "Reversión por anulación de factura"
                    };
                    await _context.KardexEntries.AddAsync(kardex);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private static string IncrementCorrelative(string current)
        {
            // Formato: 000-001-01-00000001
            var parts = current.Split('-');
            if (parts.Length == 4 && long.TryParse(parts[3], out long num))
            {
                return $"{parts[0]}-{parts[1]}-{parts[2]}-{(num + 1):D8}";
            }
            return current;
        }
    }
}
