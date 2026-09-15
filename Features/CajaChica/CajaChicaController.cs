using SmartHnl.API.Features.CajaChica.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.CajaChica
{
    [ApiController]
    [Route("api/petty-cash")]
    [Authorize]
    public class CajaChicaController : ControllerBase
    {
        private readonly CajaChicaService _service;

        public CajaChicaController(CajaChicaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCajas() => Ok(await _service.GetCajasAsync());

        [HttpGet("expenses")]
        public async Task<IActionResult> GetGastos([FromQuery] string? pettyCashId)
            => Ok(await _service.GetGastosAsync(pettyCashId));

        [HttpPost("expenses")]
        public async Task<IActionResult> CreateGasto([FromBody] CajaChicaGastoDto dto)
        {
            var res = await _service.CreateGastoAsync(dto);
            return Created($"/api/petty-cash/expenses/{res.Id}", res);
        }

        [HttpGet("documents")]
        public async Task<IActionResult> GetDocumentos() => Ok(await _service.GetDocumentosAsync());
    }
}
