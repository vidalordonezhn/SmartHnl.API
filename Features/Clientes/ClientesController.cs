using SmartHnl.API.Features.Clientes.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Clientes
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly ClientesService _service;

        public ClientsController(ClientesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var c = await _service.GetByIdAsync(id);
            if (c == null) return NotFound(new { error = "Cliente no encontrado." });
            return Ok(c);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClienteCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Created($"/api/clients/{result.Id}", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ClienteCreateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound(new { error = "Cliente no encontrado." });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound(new { error = "Cliente no encontrado." });
            return Ok(new { message = "Cliente eliminado exitosamente." });
        }
    }

    [ApiController]
    [Route("api/client-categories")]
    [Authorize]
    public class ClientCategoriesController : ControllerBase
    {
        private readonly ClientesService _service;

        public ClientCategoriesController(ClientesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetCategoriesAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClienteCategoriaDto dto)
        {
            var result = await _service.CreateCategoryAsync(dto.Name);
            return Created($"/api/client-categories/{result.Id}", result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ok = await _service.DeleteCategoryAsync(id);
            if (!ok) return NotFound(new { error = "Categoría no encontrada." });
            return Ok(new { message = "Categoría eliminada." });
        }
    }
}
