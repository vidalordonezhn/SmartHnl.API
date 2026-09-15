using SmartHnl.API.Features.CuentasCobrar.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.CuentasCobrar
{
    [ApiController]
    [Route("api/accounts-receivable")]
    [Authorize]
    public class CuentasCobrarController : ControllerBase
    {
        private readonly CuentasCobrarService _service;

        public CuentasCobrarController(CuentasCobrarService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPost("{id}/payment")]
        public async Task<IActionResult> AddPayment(string id, [FromBody] RegistrarAbonoDto dto)
        {
            var ok = await _service.AddPaymentAsync(id, dto);
            if (!ok) return BadRequest(new { error = "No se pudo registrar el abono." });
            return Ok(new { message = "Abono registrado exitosamente." });
        }
    }
}
