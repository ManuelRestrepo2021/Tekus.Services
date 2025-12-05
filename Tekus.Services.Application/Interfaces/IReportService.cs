using System.Threading.Tasks;
using Tekus.Services.Application.Dtos;

namespace Tekus.Services.Application.Interfaces
{
    /// <summary>
    /// Servicio de aplicación para obtener reportes y resúmenes de indicadores.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Obtiene un resumen con indicadores globales del sistema,
        /// como la cantidad de proveedores por país y la cantidad de servicios por país.
        /// </summary>
        Task<SummaryReportDto> GetSummaryAsync();
    }
}