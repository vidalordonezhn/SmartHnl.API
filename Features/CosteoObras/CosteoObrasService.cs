using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.CosteoObras.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.CosteoObras
{
    public class CosteoObrasService
    {
        private readonly SmartHnlDbContext _context;

        public CosteoObrasService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProyectoCostoDto>> GetAllAsync()
        {
            return await _context.ProyectosCostos
                .AsNoTracking()
                .Include(p => p.Items)
                .OrderByDescending(p => p.StartDate)
                .Select(p => new ProyectoCostoDto
                {
                    Id = p.Id,
                    Code = p.Code,
                    ProjectName = p.ProjectName,
                    QuotationId = p.QuotationId,
                    QuotationNumber = p.QuotationNumber,
                    InvoiceId = p.InvoiceId,
                    InvoiceNumber = p.InvoiceNumber,
                    ClientId = p.ClientId,
                    ClientName = p.ClientName,
                    ClientRtn = p.ClientRtn,
                    ClientPhone = p.ClientPhone,
                    ClientAddress = p.ClientAddress,
                    Responsible = p.Responsible,
                    StartDate = p.StartDate,
                    EstimatedEndDate = p.EstimatedEndDate,
                    ClosedDate = p.ClosedDate,
                    ClosedBy = p.ClosedBy,
                    ClosingNotes = p.ClosingNotes,
                    Status = p.Status,
                    QuotedAmount = p.QuotedAmount,
                    InvoicedAmount = p.InvoicedAmount,
                    TotalCost = p.Items.Sum(i => i.Amount),
                    Items = p.Items.OrderBy(i => i.Date).Select(i => new ProyectoCostoItemDto
                    {
                        Id = i.Id,
                        ProjectCostId = i.ProjectCostId,
                        Date = i.Date,
                        Category = i.Category,
                        Description = i.Description,
                        SupplierOrResponsible = i.SupplierOrResponsible,
                        VoucherType = i.VoucherType,
                        ReceiptNumber = i.ReceiptNumber,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Amount = i.Amount,
                        PaymentMethod = i.PaymentMethod,
                        Notes = i.Notes
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<ProyectoCostoDto?> GetByIdAsync(string id)
        {
            var p = await _context.ProyectosCostos
                .AsNoTracking()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null) return null;

            return new ProyectoCostoDto
            {
                Id = p.Id,
                Code = p.Code,
                ProjectName = p.ProjectName,
                QuotationId = p.QuotationId,
                QuotationNumber = p.QuotationNumber,
                InvoiceId = p.InvoiceId,
                InvoiceNumber = p.InvoiceNumber,
                ClientId = p.ClientId,
                ClientName = p.ClientName,
                ClientRtn = p.ClientRtn,
                ClientPhone = p.ClientPhone,
                ClientAddress = p.ClientAddress,
                Responsible = p.Responsible,
                StartDate = p.StartDate,
                EstimatedEndDate = p.EstimatedEndDate,
                ClosedDate = p.ClosedDate,
                ClosedBy = p.ClosedBy,
                ClosingNotes = p.ClosingNotes,
                Status = p.Status,
                QuotedAmount = p.QuotedAmount,
                InvoicedAmount = p.InvoicedAmount,
                TotalCost = p.Items.Sum(i => i.Amount),
                Items = p.Items.OrderBy(i => i.Date).Select(i => new ProyectoCostoItemDto
                {
                    Id = i.Id,
                    ProjectCostId = i.ProjectCostId,
                    Date = i.Date,
                    Category = i.Category,
                    Description = i.Description,
                    SupplierOrResponsible = i.SupplierOrResponsible,
                    VoucherType = i.VoucherType,
                    ReceiptNumber = i.ReceiptNumber,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Amount = i.Amount,
                    PaymentMethod = i.PaymentMethod,
                    Notes = i.Notes
                }).ToList()
            };
        }

        public async Task<ProyectoCostoDto> CreateAsync(ProyectoCostoDto dto)
        {
            int count = await _context.ProyectosCostos.CountAsync();
            string code = $"OBRA-{(count + 1):D6}";

            var project = new ProyectoCosto
            {
                Code = code,
                ProjectName = dto.ProjectName,
                QuotationId = dto.QuotationId,
                QuotationNumber = dto.QuotationNumber,
                InvoiceId = dto.InvoiceId,
                InvoiceNumber = dto.InvoiceNumber,
                ClientId = dto.ClientId,
                ClientName = dto.ClientName,
                ClientRtn = dto.ClientRtn,
                ClientPhone = dto.ClientPhone,
                ClientAddress = dto.ClientAddress,
                Responsible = dto.Responsible,
                StartDate = string.IsNullOrWhiteSpace(dto.StartDate) ? DateTime.Now.ToString("yyyy-MM-dd") : dto.StartDate,
                EstimatedEndDate = dto.EstimatedEndDate,
                Status = string.IsNullOrWhiteSpace(dto.Status) ? "ABIERTO" : dto.Status,
                QuotedAmount = dto.QuotedAmount,
                InvoicedAmount = dto.InvoicedAmount
            };

            foreach (var item in dto.Items)
            {
                decimal amount = item.Amount > 0 ? item.Amount : (item.Quantity * item.UnitPrice);
                project.Items.Add(new ProyectoCostoItem
                {
                    Date = string.IsNullOrWhiteSpace(item.Date) ? DateTime.Now.ToString("yyyy-MM-dd") : item.Date,
                    Category = item.Category,
                    Description = item.Description,
                    SupplierOrResponsible = item.SupplierOrResponsible,
                    VoucherType = item.VoucherType,
                    ReceiptNumber = item.ReceiptNumber,
                    Quantity = item.Quantity > 0 ? item.Quantity : 1,
                    UnitPrice = item.UnitPrice,
                    Amount = amount,
                    PaymentMethod = item.PaymentMethod,
                    Notes = item.Notes
                });
            }

            await _context.ProyectosCostos.AddAsync(project);
            await _context.SaveChangesAsync();

            return (await GetByIdAsync(project.Id))!;
        }

        public async Task<ProyectoCostoDto?> UpdateAsync(string id, ProyectoCostoDto dto)
        {
            var project = await _context.ProyectosCostos.FindAsync(id);
            if (project == null) return null;

            project.ProjectName = dto.ProjectName;
            project.QuotationId = dto.QuotationId;
            project.QuotationNumber = dto.QuotationNumber;
            project.InvoiceId = dto.InvoiceId;
            project.InvoiceNumber = dto.InvoiceNumber;
            project.ClientId = dto.ClientId;
            project.ClientName = dto.ClientName;
            project.ClientRtn = dto.ClientRtn;
            project.ClientPhone = dto.ClientPhone;
            project.ClientAddress = dto.ClientAddress;
            project.Responsible = dto.Responsible;
            project.StartDate = dto.StartDate;
            project.EstimatedEndDate = dto.EstimatedEndDate;
            project.Status = dto.Status;
            project.QuotedAmount = dto.QuotedAmount;
            project.InvoicedAmount = dto.InvoicedAmount;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task<ProyectoCostoItemDto> AddItemAsync(string projectCostId, ProyectoCostoItemDto itemDto)
        {
            var project = await _context.ProyectosCostos.FindAsync(projectCostId)
                ?? throw new Exception("Proyecto no encontrado.");

            decimal amount = itemDto.Amount > 0 ? itemDto.Amount : (itemDto.Quantity * itemDto.UnitPrice);

            var item = new ProyectoCostoItem
            {
                ProjectCostId = projectCostId,
                Date = string.IsNullOrWhiteSpace(itemDto.Date) ? DateTime.Now.ToString("yyyy-MM-dd") : itemDto.Date,
                Category = itemDto.Category,
                Description = itemDto.Description,
                SupplierOrResponsible = itemDto.SupplierOrResponsible,
                VoucherType = itemDto.VoucherType,
                ReceiptNumber = itemDto.ReceiptNumber,
                Quantity = itemDto.Quantity > 0 ? itemDto.Quantity : 1,
                UnitPrice = itemDto.UnitPrice,
                Amount = amount,
                PaymentMethod = itemDto.PaymentMethod,
                Notes = itemDto.Notes
            };

            await _context.ProyectoCostoItems.AddAsync(item);
            await _context.SaveChangesAsync();

            itemDto.Id = item.Id;
            itemDto.ProjectCostId = projectCostId;
            itemDto.Amount = item.Amount;
            return itemDto;
        }

        public async Task<bool> DeleteItemAsync(string itemId)
        {
            var item = await _context.ProyectoCostoItems.FindAsync(itemId);
            if (item == null) return false;

            _context.ProyectoCostoItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CloseProjectAsync(string id, string closedBy, string? notes)
        {
            var project = await _context.ProyectosCostos.FindAsync(id);
            if (project == null) return false;

            project.Status = "CERRADO";
            project.ClosedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            project.ClosedBy = closedBy;
            project.ClosingNotes = notes;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var project = await _context.ProyectosCostos.Include(p => p.Items).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null) return false;

            _context.ProyectoCostoItems.RemoveRange(project.Items);
            _context.ProyectosCostos.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
