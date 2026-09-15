using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.BoletasCompra.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.BoletasCompra
{
    public class BoletasCompraService
    {
        private readonly SmartHnlDbContext _context;

        public BoletasCompraService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<BoletaCompraDto>> GetAllAsync()
        {
            return await _context.BoletasCompra
                .AsNoTracking()
                .Include(b => b.Details)
                .OrderByDescending(b => b.Date)
                .Select(b => new BoletaCompraDto
                {
                    Id = b.Id,
                    BoletaNumber = b.BoletaNumber,
                    CaiId = b.CaiId,
                    CaiCode = b.CaiCode,
                    Date = b.Date,
                    ProviderId = b.ProviderId,
                    ProviderName = b.ProviderName,
                    ProviderRtn = b.ProviderRtn,
                    ProviderPhone = b.ProviderPhone,
                    ProviderAddress = b.ProviderAddress,
                    PaymentType = b.PaymentType,
                    Comments = b.Comments,
                    Subtotal = b.Subtotal,
                    TotalGeneral = b.TotalGeneral,
                    IsExonerated = b.IsExonerated,
                    Status = b.Status,
                    Details = b.Details.Select(d => new BoletaDetalleDto
                    {
                        Id = d.Id,
                        Description = d.Description,
                        Quantity = d.Quantity,
                        PriceUnit = d.PriceUnit,
                        TotalItem = d.TotalItem
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<List<CaiBoletaCompraDto>> GetCaisAsync()
        {
            return await _context.CaiBoletasCompra
                .AsNoTracking()
                .Select(c => new CaiBoletaCompraDto
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

        public async Task<CaiBoletaCompraDto> CreateCaiAsync(CaiBoletaCompraDto dto)
        {
            var cai = new AutorizacionCaiBoletaCompra
            {
                Cai = dto.Cai,
                RangoInicial = dto.RangoInicial,
                RangoFinal = dto.RangoFinal,
                CorrelativoActual = dto.CorrelativoActual,
                FechaLimite = dto.FechaLimite,
                Activo = 1
            };
            await _context.CaiBoletasCompra.AddAsync(cai);
            await _context.SaveChangesAsync();
            dto.Id = cai.Id;
            return dto;
        }

        public async Task<BoletaCompraDto> CreateBoletaCompraAsync(BoletaCompraDto dto, string userId)
        {
            var activeCai = await _context.CaiBoletasCompra
                .FirstOrDefaultAsync(c => c.Activo == 1) 
                ?? throw new Exception("No hay un CAI activo configurado para Boletas de Compra.");

            string nextNum = IncrementCorrelative(activeCai.CorrelativoActual);
            activeCai.CorrelativoActual = nextNum;

            decimal total = dto.Details.Sum(d => d.Quantity * d.PriceUnit);

            var boleta = new BoletaCompra
            {
                BoletaNumber = nextNum,
                CaiId = activeCai.Id,
                CaiCode = activeCai.Cai,
                Date = string.IsNullOrWhiteSpace(dto.Date) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : dto.Date,
                ProviderId = dto.ProviderId,
                ProviderName = dto.ProviderName ?? "",
                ProviderRtn = dto.ProviderRtn ?? "",
                ProviderPhone = dto.ProviderPhone,
                ProviderAddress = dto.ProviderAddress,
                PaymentType = dto.PaymentType ?? "CONTADO",
                Comments = dto.Comments,
                Subtotal = total,
                TotalGeneral = total,
                IsExonerated = dto.IsExonerated,
                UserId = userId,
                Status = "EMITIDA"
            };

            foreach (var item in dto.Details)
            {
                boleta.Details.Add(new BoletaCompraDetalle
                {
                    Description = item.Description,
                    Quantity = item.Quantity,
                    PriceUnit = item.PriceUnit,
                    TotalItem = item.Quantity * item.PriceUnit
                });
            }

            await _context.BoletasCompra.AddAsync(boleta);
            await _context.SaveChangesAsync();

            dto.Id = boleta.Id;
            dto.BoletaNumber = boleta.BoletaNumber;
            dto.CaiId = boleta.CaiId;
            dto.CaiCode = boleta.CaiCode;
            dto.Subtotal = boleta.Subtotal;
            dto.TotalGeneral = boleta.TotalGeneral;
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
