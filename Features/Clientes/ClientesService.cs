using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.Clientes.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Clientes
{
    public class ClientesService
    {
        private readonly SmartHnlDbContext _context;

        public ClientesService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClienteDto>> GetAllAsync()
        {
            return await _context.Clientes
                .AsNoTracking()
                .Include(c => c.Category)
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Rtn = c.Rtn,
                    Email = c.Email,
                    Phone = c.Phone,
                    Address = c.Address,
                    ExonerationActive = c.ExonerationActive,
                    CreditLimit = c.CreditLimit,
                    Status = c.Status,
                    CategoryId = c.CategoryId,
                    CategoryName = c.Category != null ? c.Category.Name : null
                }).ToListAsync();
        }

        public async Task<ClienteDto?> GetByIdAsync(string id)
        {
            return await _context.Clientes
                .AsNoTracking()
                .Include(c => c.Category)
                .Where(c => c.Id == id)
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Rtn = c.Rtn,
                    Email = c.Email,
                    Phone = c.Phone,
                    Address = c.Address,
                    ExonerationActive = c.ExonerationActive,
                    CreditLimit = c.CreditLimit,
                    Status = c.Status,
                    CategoryId = c.CategoryId,
                    CategoryName = c.Category != null ? c.Category.Name : null
                }).FirstOrDefaultAsync();
        }

        public async Task<ClienteDto> CreateAsync(ClienteCreateDto dto)
        {
            var cliente = new Cliente
            {
                Name = dto.Name.Trim(),
                Rtn = dto.Rtn.Trim(),
                Email = dto.Email?.Trim() ?? "",
                Phone = dto.Phone?.Trim() ?? "",
                Address = dto.Address?.Trim() ?? "",
                ExonerationActive = dto.ExonerationActive,
                CreditLimit = dto.CreditLimit,
                Status = dto.Status ?? "ACTIVO",
                CategoryId = dto.CategoryId
            };

            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return (await GetByIdAsync(cliente.Id))!;
        }

        public async Task<ClienteDto?> UpdateAsync(string id, ClienteCreateDto dto)
        {
            var c = await _context.Clientes.FindAsync(id);
            if (c == null) return null;

            c.Name = dto.Name.Trim();
            c.Rtn = dto.Rtn.Trim();
            c.Email = dto.Email?.Trim() ?? "";
            c.Phone = dto.Phone?.Trim() ?? "";
            c.Address = dto.Address?.Trim() ?? "";
            c.ExonerationActive = dto.ExonerationActive;
            c.CreditLimit = dto.CreditLimit;
            if (dto.Status != null) c.Status = dto.Status;
            c.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
            return (await GetByIdAsync(id))!;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var c = await _context.Clientes.FindAsync(id);
            if (c == null) return false;
            _context.Clientes.Remove(c);
            await _context.SaveChangesAsync();
            return true;
        }

        // Categorías de clientes
        public async Task<List<ClienteCategoriaDto>> GetCategoriesAsync()
        {
            return await _context.ClienteCategorias
                .AsNoTracking()
                .Select(cc => new ClienteCategoriaDto { Id = cc.Id, Name = cc.Name })
                .ToListAsync();
        }

        public async Task<ClienteCategoriaDto> CreateCategoryAsync(string name)
        {
            var cat = new ClienteCategoria { Name = name };
            await _context.ClienteCategorias.AddAsync(cat);
            await _context.SaveChangesAsync();
            return new ClienteCategoriaDto { Id = cat.Id, Name = cat.Name };
        }

        public async Task<bool> DeleteCategoryAsync(string id)
        {
            var cat = await _context.ClienteCategorias.FindAsync(id);
            if (cat == null) return false;
            _context.ClienteCategorias.Remove(cat);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
