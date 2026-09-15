using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Configuracion
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class ConfiguracionController : ControllerBase
    {
        private readonly SmartHnlDbContext _context;

        public ConfiguracionController(SmartHnlDbContext context)
        {
            _context = context;
        }

        [HttpGet("settings")]
        public async Task<IActionResult> GetSettings()
        {
            var s = await _context.Configuracion.FirstOrDefaultAsync(c => c.Id == 1);
            return Ok(s);
        }

        [HttpPut("settings")]
        public async Task<IActionResult> UpdateSettings([FromBody] ConfiguracionEmpresa dto)
        {
            var s = await _context.Configuracion.FirstOrDefaultAsync(c => c.Id == 1);
            if (s == null) return NotFound();

            s.Name = dto.Name;
            s.CommercialName = dto.CommercialName;
            s.DisplayNameType = dto.DisplayNameType;
            s.Rtn = dto.Rtn;
            s.Address = dto.Address;
            s.Phone = dto.Phone;
            s.Email = dto.Email;
            s.LogoUrl = dto.LogoUrl;
            s.Cai = dto.Cai;
            s.RangeFrom = dto.RangeFrom;
            s.RangeTo = dto.RangeTo;
            s.ExpiryDate = dto.ExpiryDate;
            s.CurrentInvoiceNumber = dto.CurrentInvoiceNumber;
            s.Location = dto.Location;
            s.HonorariosCai = dto.HonorariosCai;
            s.HonorariosRangeFrom = dto.HonorariosRangeFrom;
            s.HonorariosRangeTo = dto.HonorariosRangeTo;
            s.HonorariosExpiryDate = dto.HonorariosExpiryDate;
            s.HonorariosCurrentNumber = dto.HonorariosCurrentNumber;
            s.HideHonorarios = dto.HideHonorarios;

            await _context.SaveChangesAsync();
            return Ok(s);
        }

        [HttpGet("payment-terms")]
        public async Task<IActionResult> GetPaymentTerms()
        {
            var terms = await _context.TerminosPago.AsNoTracking().ToListAsync();
            return Ok(terms);
        }
    }
}
