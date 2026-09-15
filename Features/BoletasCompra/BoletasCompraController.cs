using SmartHnl.API.Features.BoletasCompra.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.BoletasCompra
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class BoletasCompraController : ControllerBase
    {
        private readonly BoletasCompraService _service;

        public BoletasCompraController(BoletasCompraService service)
        {
            _service = service;
        }

        [HttpGet("boletas-compra")]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPost("boletas-compra")]
        public async Task<IActionResult> Create([FromBody] BoletaCompraDto dto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "admin-id";
            var result = await _service.CreateBoletaCompraAsync(dto, userId);
            return Created($"/api/boletas-compra/{result.Id}", result);
        }

        [HttpGet("boleta-compra-cai")]
        public async Task<IActionResult> GetCais() => Ok(await _service.GetCaisAsync());

        [HttpPost("boleta-compra-cai")]
        public async Task<IActionResult> CreateCai([FromBody] CaiBoletaCompraDto dto)
        {
            var result = await _service.CreateCaiAsync(dto);
            return Created($"/api/boleta-compra-cai/{result.Id}", result);
        }
    }
}
