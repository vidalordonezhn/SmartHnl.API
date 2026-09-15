using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.Categorias.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Categorias
{
    public class CategoriasService
    {
        private readonly SmartHnlDbContext _context;

        public CategoriasService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaDto>> GetAllAsync()
        {
            return await _context.Categorias
                .AsNoTracking()
                .Select(c => new CategoriaDto { Id = c.Id, Name = c.Name })
                .ToListAsync();
        }

        public async Task<CategoriaDto> CreateAsync(CategoriaCreateDto dto)
        {
            var c = new Categoria { Name = dto.Name.Trim() };
            await _context.Categorias.AddAsync(c);
            await _context.SaveChangesAsync();
            return new CategoriaDto { Id = c.Id, Name = c.Name };
        }

        public async Task<CategoriaDto?> UpdateAsync(string id, CategoriaCreateDto dto)
        {
            var c = await _context.Categorias.FindAsync(id);
            if (c == null) return null;
            c.Name = dto.Name.Trim();
            await _context.SaveChangesAsync();
            return new CategoriaDto { Id = c.Id, Name = c.Name };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var c = await _context.Categorias.FindAsync(id);
            if (c == null) return false;
            _context.Categorias.Remove(c);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
