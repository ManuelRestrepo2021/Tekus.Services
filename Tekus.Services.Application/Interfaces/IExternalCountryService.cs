using System.Collections.Generic;
using System.Threading.Tasks;
using Tekus.Services.Application.Dtos;

namespace Tekus.Services.Application.Interfaces
{
    /// <summary>
    /// Servicio que obtiene países desde una fuente externa (simulada o real).
    /// </summary>
    public interface IExternalCountryService
    {
        /// <summary>
        /// Obtiene la lista de países desde el servicio externo.
        /// </summary>
        Task<IReadOnlyList<ExternalCountryDto>> GetCountriesAsync();
    }
}