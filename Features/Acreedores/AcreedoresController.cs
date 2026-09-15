using SmartHnl.API.Features.Acreedores;
using SmartHnl.API.Features.Proveedores.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Acreedores
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AcreedoresController : ControllerBase
    {
        private readonly AcreedoresService _service;

        public AcreedoresController(AcreedoresService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProveedorCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Created($"/api/acreedores/{result.Id}", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ProveedorCreateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound(new { error = "Acreedor no encontrado." });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound(new { error = "Acreedor no encontrado." });
            return Ok(new { message = "Acreedor eliminado." });
        }
    }
}
