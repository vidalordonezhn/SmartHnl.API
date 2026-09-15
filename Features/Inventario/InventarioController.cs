using SmartHnl.API.Features.Inventario.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Inventario
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class InventarioController : ControllerBase
    {
        private readonly InventarioService _service;

        public InventarioController(InventarioService service)
        {
            _service = service;
        }

        [HttpGet("kardex")]
        public async Task<IActionResult> GetKardex([FromQuery] string? productId)
            => Ok(await _service.GetKardexAsync(productId));

        [HttpPost("inventory/adjustment")]
        public async Task<IActionResult> Adjust([FromBody] InventoryAdjustmentDto dto)
        {
            var ok = await _service.AdjustInventoryAsync(dto);
            if (!ok) return BadRequest(new { error = "No se pudo realizar el ajuste de inventario." });
            return Ok(new { message = "Ajuste registrado exitosamente." });
        }

        [HttpPost("inventory/recalculate")]
        public async Task<IActionResult> Recalculate()
        {
            await _service.RecalculateInventoryAsync();
            return Ok(new { message = "Inventario recalculado correctamente." });
        }

        [HttpGet("series")]
        public async Task<IActionResult> GetSeries([FromQuery] string? productId)
            => Ok(await _service.GetSeriesAsync(productId));

        [HttpPost("series")]
        public async Task<IActionResult> CreateSerie([FromBody] SerieDto dto)
        {
            var res = await _service.CreateSerieAsync(dto);
            return Created($"/api/series/{res.Id}", res);
        }
    }
}
