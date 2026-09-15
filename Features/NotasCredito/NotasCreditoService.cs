using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.NotasCredito.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.NotasCredito
{
    public class NotasCreditoService
    {
        private readonly SmartHnlDbContext _context;

        public NotasCreditoService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<NotaCreditoDto>> GetAllAsync()
        {
            return await _context.NotasCredito
                .AsNoTracking()
                .Include(n => n.Client)
                .OrderByDescending(n => n.Date)
                .Select(n => new NotaCreditoDto
                {
                    Id = n.Id,
                    CreditNoteNumber = n.CreditNoteNumber,
                    CaiId = n.CaiId,
                    CaiCode = n.CaiCode,
                    Date = n.Date,
                    ClientId = n.ClientId,
                    ClientName = n.Client != null ? n.Client.Name : null,
                    InvoiceId = n.InvoiceId,
                    InvoiceNumber = n.InvoiceNumber,
                    NoteType = n.NoteType,
                    Comments = n.Comments,
                    SubtotalGravado = n.SubtotalGravado,
                    SubtotalExento = n.SubtotalExento,
                    SubtotalExonerado = n.SubtotalExonerado,
                    IsvTotal = n.IsvTotal,
                    TotalGeneral = n.TotalGeneral,
                    Status = n.Status
                }).ToListAsync();
        }

        public async Task<List<CaiNotaCreditoDto>> GetCaisAsync()
        {
            return await _context.CaiNotasCredito
                .AsNoTracking()
                .Select(c => new CaiNotaCreditoDto
                {
                    Id = c.Id,
                    Cai = c.Cai,
                    RangoInicial = c.RangoInicial,
                    RangoFinal = c.RangoFinal,
                    CorrelativoActual = c.CorrelativoActual,
                    FechaLimite = c.FechaLimite,
                    Activo = c.Activo
                }).ToListAsync();
        }

        public async Task<CaiNotaCreditoDto> CreateCaiAsync(CaiNotaCreditoDto dto)
        {
            var cai = new AutorizacionCaiNotaCredito
            {
                Cai = dto.Cai,
                RangoInicial = dto.RangoInicial,
                RangoFinal = dto.RangoFinal,
                CorrelativoActual = dto.CorrelativoActual,
                FechaLimite = dto.FechaLimite,
                Activo = 1
            };
            await _context.CaiNotasCredito.AddAsync(cai);
            await _context.SaveChangesAsync();
            dto.Id = cai.Id;
            return dto;
        }

        public async Task<NotaCreditoDto> CreateNotaCreditoAsync(NotaCreditoDto dto, string userId)
        {
            var activeCai = await _context.CaiNotasCredito
                .FirstOrDefaultAsync(c => c.Activo == 1) 
                ?? throw new Exception("No hay un CAI activo configurado para Notas de Crédito.");

            string nextNum = IncrementCorrelative(activeCai.CorrelativoActual);
            activeCai.CorrelativoActual = nextNum;

            var nc = new NotaCredito
            {
                CreditNoteNumber = nextNum,
                CaiId = activeCai.Id,
                CaiCode = activeCai.Cai,
                Date = string.IsNullOrWhiteSpace(dto.Date) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : dto.Date,
                ClientId = dto.ClientId,
                InvoiceId = dto.InvoiceId,
                InvoiceNumber = dto.InvoiceNumber,
                NoteType = string.IsNullOrWhiteSpace(dto.NoteType) ? "ANULACION" : dto.NoteType,
                Comments = dto.Comments ?? "",
                SubtotalGravado = dto.SubtotalGravado,
                SubtotalExento = dto.SubtotalExento,
                SubtotalExonerado = dto.SubtotalExonerado,
                IsvTotal = dto.IsvTotal,
                TotalGeneral = dto.TotalGeneral,
                UserId = userId,
                Status = "EMITIDA"
            };

            // Si es anulación de factura, actualizar el estado de la factura a ANULADA y revertir stock
            if (nc.NoteType == "ANULACION" && !string.IsNullOrEmpty(nc.InvoiceId))
            {
                var inv = await _context.Facturas
                    .Include(f => f.Details)
                    .FirstOrDefaultAsync(f => f.Id == nc.InvoiceId);

                if (inv != null && inv.Status != "ANULADA")
                {
                    inv.Status = "ANULADA";
                    foreach (var d in inv.Details)
                    {
                        var prod = await _context.Productos.FindAsync(d.ProductId);
                        if (prod != null && prod.IsService == 0)
                        {
                            prod.Stock += d.Quantity;
                            await _context.KardexEntries.AddAsync(new Kardex
                            {
                                ProductId = prod.Id,
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                Type = "ENTRADA",
                                Quantity = d.Quantity,
                                CostUnit = d.CostUnit,
                                StockAfter = prod.Stock,
                                Reference = $"NC {nextNum} -> ANULACION {inv.InvoiceNumber}",
                                Notes = "Reversión de inventario por emisión de Nota de Crédito"
                            });
                        }
                    }
                }
            }

            await _context.NotasCredito.AddAsync(nc);
            await _context.SaveChangesAsync();

            dto.Id = nc.Id;
            dto.CreditNoteNumber = nc.CreditNoteNumber;
            dto.CaiId = nc.CaiId;
            dto.CaiCode = nc.CaiCode;
            return dto;
        }

        private static string IncrementCorrelative(string current)
        {
            var parts = current.Split('-');
            if (parts.Length == 4 && long.TryParse(parts[3], out long num))
            {
                return $"{parts[0]}-{parts[1]}-{parts[2]}-{(num + 1):D8}";
            }
            return current;
        }
    }
}
