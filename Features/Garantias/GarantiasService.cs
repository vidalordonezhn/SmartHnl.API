using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Garantias
{
    public class GarantiasService
    {
        private readonly SmartHnlDbContext _context;

        public GarantiasService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<Garantia>> GetAllAsync()
        {
            return await _context.Garantias
                .AsNoTracking()
                .Include(g => g.Cliente)
                .Include(g => g.Producto)
                .Include(g => g.Factura)
                .ToListAsync();
        }
    }
}
