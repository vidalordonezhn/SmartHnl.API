using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.Proveedores.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Acreedores
{
    public class AcreedoresService
    {
        private readonly SmartHnlDbContext _context;

        public AcreedoresService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProveedorDto>> GetAllAsync()
        {
            return await _context.Acreedores
                .AsNoTracking()
                .Select(a => new ProveedorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Rtn = a.Rtn,
                    Email = a.Email,
                    Phone = a.Phone,
                    Address = a.Address,
                    ContactName = a.ContactName
                }).ToListAsync();
        }

        public async Task<ProveedorDto> CreateAsync(ProveedorCreateDto dto)
        {
            var a = new Acreedor
            {
                Name = dto.Name.Trim(),
                Rtn = dto.Rtn.Trim(),
                Email = dto.Email?.Trim() ?? "",
                Phone = dto.Phone?.Trim() ?? "",
                Address = dto.Address?.Trim() ?? "",
                ContactName = dto.ContactName?.Trim() ?? ""
            };

            await _context.Acreedores.AddAsync(a);
            await _context.SaveChangesAsync();
            return new ProveedorDto
            {
                Id = a.Id,
                Name = a.Name,
                Rtn = a.Rtn,
                Email = a.Email,
                Phone = a.Phone,
                Address = a.Address,
                ContactName = a.ContactName
            };
        }

        public async Task<ProveedorDto?> UpdateAsync(string id, ProveedorCreateDto dto)
        {
            var a = await _context.Acreedores.FindAsync(id);
            if (a == null) return null;

            a.Name = dto.Name.Trim();
            a.Rtn = dto.Rtn.Trim();
            a.Email = dto.Email?.Trim() ?? "";
            a.Phone = dto.Phone?.Trim() ?? "";
            a.Address = dto.Address?.Trim() ?? "";
            a.ContactName = dto.ContactName?.Trim() ?? "";

            await _context.SaveChangesAsync();
            return new ProveedorDto
            {
                Id = a.Id,
                Name = a.Name,
                Rtn = a.Rtn,
                Email = a.Email,
                Phone = a.Phone,
                Address = a.Address,
                ContactName = a.ContactName
            };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var a = await _context.Acreedores.FindAsync(id);
            if (a == null) return false;
            _context.Acreedores.Remove(a);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
