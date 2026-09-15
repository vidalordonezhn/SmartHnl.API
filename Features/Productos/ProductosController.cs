using SmartHnl.API.Features.Productos.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Productos
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ProductosService _service;

        public ProductsController(ProductosService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var p = await _service.GetByIdAsync(id);
            if (p == null) return NotFound(new { error = "Producto no encontrado." });
            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductoCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Created($"/api/products/{result.Id}", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ProductoCreateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound(new { error = "Producto no encontrado." });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound(new { error = "Producto no encontrado." });
            return Ok(new { message = "Producto eliminado." });
        }
    }
}
