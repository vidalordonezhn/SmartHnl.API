using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.Proveedores.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Proveedores
{
    public class ProveedoresService
    {
        private readonly SmartHnlDbContext _context;

        public ProveedoresService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProveedorDto>> GetAllAsync()
        {
            return await _context.Proveedores
                .AsNoTracking()
                .Select(p => new ProveedorDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Rtn = p.Rtn,
                    Email = p.Email,
                    Phone = p.Phone,
                    Address = p.Address,
                    ContactName = p.ContactName
                }).ToListAsync();
        }

        public async Task<ProveedorDto?> GetByIdAsync(string id)
        {
            return await _context.Proveedores
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProveedorDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Rtn = p.Rtn,
                    Email = p.Email,
                    Phone = p.Phone,
                    Address = p.Address,
                    ContactName = p.ContactName
                }).FirstOrDefaultAsync();
        }

        public async Task<ProveedorDto> CreateAsync(ProveedorCreateDto dto)
        {
            var p = new Proveedor
            {
                Name = dto.Name.Trim(),
                Rtn = dto.Rtn.Trim(),
                Email = dto.Email?.Trim() ?? "",
                Phone = dto.Phone?.Trim() ?? "",
                Address = dto.Address?.Trim() ?? "",
                ContactName = dto.ContactName?.Trim() ?? ""
            };

            await _context.Proveedores.AddAsync(p);
            await _context.SaveChangesAsync();
            return (await GetByIdAsync(p.Id))!;
        }

        public async Task<ProveedorDto?> UpdateAsync(string id, ProveedorCreateDto dto)
        {
            var p = await _context.Proveedores.FindAsync(id);
            if (p == null) return null;

            p.Name = dto.Name.Trim();
            p.Rtn = dto.Rtn.Trim();
            p.Email = dto.Email?.Trim() ?? "";
            p.Phone = dto.Phone?.Trim() ?? "";
            p.Address = dto.Address?.Trim() ?? "";
            p.ContactName = dto.ContactName?.Trim() ?? "";

            await _context.SaveChangesAsync();
            return (await GetByIdAsync(id))!;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var p = await _context.Proveedores.FindAsync(id);
            if (p == null) return false;
            _context.Proveedores.Remove(p);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
