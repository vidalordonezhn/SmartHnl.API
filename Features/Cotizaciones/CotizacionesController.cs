using SmartHnl.API.Features.Cotizaciones.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Cotizaciones
{
    [ApiController]
    [Route("api/quotations")]
    [Authorize]
    public class QuotationsController : ControllerBase
    {
        private readonly CotizacionesService _service;

        public QuotationsController(CotizacionesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CotizacionDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "admin-id";
            var result = await _service.CreateAsync(dto, userId);
            return Created($"/api/quotations/{result.Id}", result);
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(string id)
        {
            var userName = User.Identity?.Name ?? "Admin";
            var ok = await _service.UpdateStatusAsync(id, "APROBADA", userName: userName);
            if (!ok) return NotFound(new { error = "Cotización no encontrada." });
            return Ok(new { message = "Cotización aprobada." });
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(string id, [FromBody] dynamic body)
        {
            string reason = body?.reason ?? "";
            var userName = User.Identity?.Name ?? "Admin";
            var ok = await _service.UpdateStatusAsync(id, "RECHAZADA", reason, userName);
            if (!ok) return NotFound(new { error = "Cotización no encontrada." });
            return Ok(new { message = "Cotización rechazada." });
        }
    }
}
