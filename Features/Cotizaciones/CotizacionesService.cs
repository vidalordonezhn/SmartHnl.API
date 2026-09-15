using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.Cotizaciones.DTOs;
using SmartHnl.API.Features.Facturacion.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Cotizaciones
{
    public class CotizacionesService
    {
        private readonly SmartHnlDbContext _context;

        public CotizacionesService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<CotizacionDto>> GetAllAsync()
        {
            return await _context.Cotizaciones
                .AsNoTracking()
                .Include(q => q.Client)
                .Include(q => q.Details)
                    .ThenInclude(d => d.Product)
                .OrderByDescending(q => q.Date)
                .Select(q => new CotizacionDto
                {
                    Id = q.Id,
                    QuotationNumber = q.QuotationNumber,
                    DocumentType = q.DocumentType,
                    Date = q.Date,
                    ClientId = q.ClientId,
                    ClientName = q.CustomClientName ?? (q.Client != null ? q.Client.Name : null),
                    ClientRtn = q.CustomClientRtn ?? (q.Client != null ? q.Client.Rtn : null),
                    Status = q.Status,
                    ValidityDays = q.ValidityDays,
                    PaymentType = q.PaymentType,
                    PaymentTerm = q.PaymentTerm,
                    IsExonerated = q.IsExonerated,
                    SubtotalGravado = q.SubtotalGravado,
                    SubtotalExento = q.SubtotalExento,
                    SubtotalExonerado = q.SubtotalExonerado,
                    IsvTotal = q.IsvTotal,
                    TotalGeneral = q.TotalGeneral,
                    Comments = q.Comments,
                    UserId = q.UserId,
                    InvoiceId = q.InvoiceId,
                    ApprovedBy = q.ApprovedBy,
                    ApprovedAt = q.ApprovedAt,
                    RejectedBy = q.RejectedBy,
                    RejectedAt = q.RejectedAt,
                    RejectionReason = q.RejectionReason,
                    Details = q.Details.Select(d => new FacturaDetalleDto
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
                        CustomDescription = d.CustomDescription,
                        Discount = d.Discount
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<CotizacionDto> CreateAsync(CotizacionDto dto, string userId)
        {
            int count = await _context.Cotizaciones.CountAsync();
            string num = $"COT-{(count + 1):D6}";

            var cot = new Cotizacion
            {
                QuotationNumber = num,
                DocumentType = dto.DocumentType,
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ClientId = dto.ClientId,
                Status = "PENDIENTE",
                ValidityDays = dto.ValidityDays > 0 ? dto.ValidityDays : 15,
                PaymentType = dto.PaymentType,
                PaymentTerm = dto.PaymentTerm,
                IsExonerated = dto.IsExonerated,
                Comments = dto.Comments,
                UserId = userId,
                CustomClientName = dto.CustomClientName,
                CustomClientRtn = dto.CustomClientRtn,
                CustomClientAddress = dto.CustomClientAddress,
                CustomClientPhone = dto.CustomClientPhone,
                CustomClientEmail = dto.CustomClientEmail
            };

            decimal sGravado = 0, sExento = 0, sExon = 0, isvTot = 0;

            foreach (var item in dto.Details)
            {
                decimal gross = (item.Quantity * item.PriceUnit) - item.Discount;
                decimal mGrav = 0, mExen = 0, mExon = 0, isv = 0;

                if (dto.IsExonerated == 1) mExon = gross;
                else if (item.TaxType == "EXENTO") mExen = gross;
                else if (item.TaxType == "GRAVADO_18") { mGrav = gross; isv = Math.Round(gross * 0.18m, 2); }
                else { mGrav = gross; isv = Math.Round(gross * 0.15m, 2); }

                sGravado += mGrav;
                sExento += mExen;
                sExon += mExon;
                isvTot += isv;

                cot.Details.Add(new CotizacionDetalle
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    PriceUnit = item.PriceUnit,
                    TaxType = item.TaxType,
                    MontoGravado = mGrav,
                    MontoExento = mExen,
                    MontoExonerado = mExon,
                    IsvCalculated = isv,
                    CustomDescription = item.CustomDescription,
                    Discount = item.Discount
                });
            }

            cot.SubtotalGravado = sGravado;
            cot.SubtotalExento = sExento;
            cot.SubtotalExonerado = sExon;
            cot.IsvTotal = isvTot;
            cot.TotalGeneral = sGravado + sExento + sExon + isvTot;

            await _context.Cotizaciones.AddAsync(cot);
            await _context.SaveChangesAsync();

            dto.Id = cot.Id;
            dto.QuotationNumber = cot.QuotationNumber;
            dto.TotalGeneral = cot.TotalGeneral;
            return dto;
        }

        public async Task<bool> UpdateStatusAsync(string id, string status, string? reason = null, string? userName = null)
        {
            var q = await _context.Cotizaciones.FindAsync(id);
            if (q == null) return false;

            q.Status = status;
            if (status == "APROBADA")
            {
                q.ApprovedBy = userName ?? "Admin";
                q.ApprovedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (status == "RECHAZADA")
            {
                q.RejectedBy = userName ?? "Admin";
                q.RejectedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                q.RejectionReason = reason;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
