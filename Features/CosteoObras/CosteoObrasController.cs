using SmartHnl.API.Features.CosteoObras;
using SmartHnl.API.Features.CosteoObras.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.CosteoObras
{
    [ApiController]
    [Route("api/project-costs")]
    [Authorize]
    public class CosteoObrasController : ControllerBase
    {
        private readonly CosteoObrasService _service;

        public CosteoObrasController(CosteoObrasService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var res = await _service.GetByIdAsync(id);
            if (res == null) return NotFound(new { error = "Proyecto de costeo no encontrado." });
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProyectoCostoDto dto)
        {
            var res = await _service.CreateAsync(dto);
            return Created($"/api/project-costs/{res.Id}", res);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ProyectoCostoDto dto)
        {
            var res = await _service.UpdateAsync(id, dto);
            if (res == null) return NotFound(new { error = "Proyecto no encontrado." });
            return Ok(res);
        }

        [HttpPost("{id}/items")]
        public async Task<IActionResult> AddItem(string id, [FromBody] ProyectoCostoItemDto itemDto)
        {
            var res = await _service.AddItemAsync(id, itemDto);
            return Created($"/api/project-costs/{id}/items/{res.Id}", res);
        }

        [HttpDelete("{id}/items/{itemId}")]
        public async Task<IActionResult> DeleteItem(string id, string itemId)
        {
            var ok = await _service.DeleteItemAsync(itemId);
            if (!ok) return NotFound(new { error = "Partida no encontrada." });
            return Ok(new { message = "Partida eliminada." });
        }

        [HttpPut("{id}/close")]
        public async Task<IActionResult> Close(string id, [FromBody] dynamic body)
        {
            var userName = User.Identity?.Name ?? "Admin";
            string notes = body?.notes ?? "";
            var ok = await _service.CloseProjectAsync(id, userName, notes);
            if (!ok) return NotFound(new { error = "Proyecto no encontrado." });
            return Ok(new { message = "Proyecto cerrado y liquidado exitosamente." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound(new { error = "Proyecto no encontrado." });
            return Ok(new { message = "Proyecto eliminado." });
        }
    }
}
