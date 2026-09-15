using SmartHnl.API.Features.Garantias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Garantias
{
    [ApiController]
    [Route("api/garantias")]
    [Authorize]
    public class GarantiasController : ControllerBase
    {
        private readonly GarantiasService _service;

        public GarantiasController(GarantiasService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
    }
}
