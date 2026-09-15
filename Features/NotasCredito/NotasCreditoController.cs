using SmartHnl.API.Features.NotasCredito.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.NotasCredito
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class NotasCreditoController : ControllerBase
    {
        private readonly NotasCreditoService _service;

        public NotasCreditoController(NotasCreditoService service)
        {
            _service = service;
        }

        [HttpGet("credit-notes")]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPost("credit-notes")]
        public async Task<IActionResult> Create([FromBody] NotaCreditoDto dto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "admin-id";
            var result = await _service.CreateNotaCreditoAsync(dto, userId);
            return Created($"/api/credit-notes/{result.Id}", result);
        }

        [HttpGet("credit-note-cai")]
        public async Task<IActionResult> GetCais() => Ok(await _service.GetCaisAsync());

        [HttpPost("credit-note-cai")]
        public async Task<IActionResult> CreateCai([FromBody] CaiNotaCreditoDto dto)
        {
            var result = await _service.CreateCaiAsync(dto);
            return Created($"/api/credit-note-cai/{result.Id}", result);
        }
    }
}
