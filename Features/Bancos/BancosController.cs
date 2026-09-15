using SmartHnl.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Bancos
{
    [ApiController]
    [Route("api/banks")]
    [Authorize]
    public class BancosController : ControllerBase
    {
        private readonly SmartHnlDbContext _context;

        public BancosController(SmartHnlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.Bancos.AsNoTracking().ToListAsync());
    }
}
