using SmartHnl.API.Features.CuentasPagar.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.CuentasPagar
{
    [ApiController]
    [Route("api/accounts-payable")]
    [Authorize]
    public class CuentasPagarController : ControllerBase
    {
        private readonly CuentasPagarService _service;

        public CuentasPagarController(CuentasPagarService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPost("{id}/payment")]
        public async Task<IActionResult> AddPayment(string id, [FromBody] RegistrarPagoProveedorDto dto)
        {
            var ok = await _service.AddPaymentAsync(id, dto);
            if (!ok) return BadRequest(new { error = "No se pudo registrar el pago al proveedor." });
            return Ok(new { message = "Pago registrado exitosamente." });
        }
    }
}
