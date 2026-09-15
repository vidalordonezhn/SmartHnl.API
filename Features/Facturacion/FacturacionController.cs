using SmartHnl.API.Features.Facturacion.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Facturacion
{
    [ApiController]
    [Route("api/invoices")]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly FacturacionService _service;

        public InvoicesController(FacturacionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? establishment = "ALL")
            => Ok(await _service.GetAllAsync(establishment));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var res = await _service.GetByIdAsync(id);
            if (res == null) return NotFound(new { error = "Factura no encontrada." });
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FacturaCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "admin-id";
            var result = await _service.CreateInvoiceAsync(dto, userId);
            return Created($"/api/invoices/{result.Id}", result);
        }

        [HttpPost("{id}/annul")]
        public async Task<IActionResult> Annul(string id)
        {
            var ok = await _service.AnnulInvoiceAsync(id);
            if (!ok) return BadRequest(new { error = "No se pudo anular la factura." });
            return Ok(new { message = "Factura anulada exitosamente." });
        }
    }
}
