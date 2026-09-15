using SmartHnl.API.Features.Compras.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Compras
{
    [ApiController]
    [Route("api/purchases")]
    [Authorize]
    public class PurchasesController : ControllerBase
    {
        private readonly ComprasService _service;

        public PurchasesController(ComprasService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompraCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Created($"/api/purchases/{result.Id}", result);
        }
    }
}
