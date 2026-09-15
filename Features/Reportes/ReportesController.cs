using SmartHnl.API.Features.Reportes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SmartHnl.API.Features.Reportes
{
    [ApiController]
    [Route("api/reports")]
    [Authorize]
    public class ReportesController : ControllerBase
    {
        private readonly ReportesService _service;

        public ReportesController(ReportesService service)
        {
            _service = service;
        }

        [HttpGet("utility-consolidated")]
        public async Task<IActionResult> GetUtilityReport(
            [FromQuery] string? establishment = "ALL",
            [FromQuery] string? startDate = null,
            [FromQuery] string? endDate = null)
        {
            var res = await _service.GetReporteUtilidadConsolidadoAsync(establishment, startDate, endDate);
            return Ok(res);
        }

        [HttpGet("daily-cash-audit")]
        public async Task<IActionResult> GetDailyCashAudit(
            [FromQuery] string date,
            [FromQuery] string? establishment = "ALL")
        {
            var res = await _service.GetArqueoDiarioAsync(date, establishment);
            return Ok(res);
        }
    }
}
