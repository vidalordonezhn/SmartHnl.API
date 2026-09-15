using SmartHnl.API.Features.Proveedores.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Proveedores
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProvidersController : ControllerBase
    {
        private readonly ProveedoresService _service;

        public ProvidersController(ProveedoresService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var p = await _service.GetByIdAsync(id);
            if (p == null) return NotFound(new { error = "Proveedor no encontrado." });
            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProveedorCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Created($"/api/providers/{result.Id}", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ProveedorCreateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound(new { error = "Proveedor no encontrado." });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound(new { error = "Proveedor no encontrado." });
            return Ok(new { message = "Proveedor eliminado." });
        }
    }
}
