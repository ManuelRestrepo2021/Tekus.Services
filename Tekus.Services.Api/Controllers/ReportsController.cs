using Microsoft.AspNetCore.Mvc;
using Tekus.Services.Application.Dtos;
using Tekus.Services.Application.Interfaces;

namespace Tekus.Services.Api.Controllers
{
    /// <summary>
    /// Controlador para exponer reportes y resúmenes de indicadores del sistema.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Obtiene un resumen con indicadores:
        /// - Cantidad de proveedores por país.
        /// - Cantidad de servicios distintos ofrecidos por país.
        /// </summary>
        [HttpGet("summary")]
        [ProducesResponseType(typeof(SummaryReportDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<SummaryReportDto>> GetSummaryAsync()
        {
            var summary = await _reportService.GetSummaryAsync();
            return Ok(summary);
        }
    }
}