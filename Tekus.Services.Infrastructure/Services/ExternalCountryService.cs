using Tekus.Services.Application.Dtos;
using Tekus.Services.Application.Interfaces;

namespace Tekus.Services.Infrastructure.Services
{
    /// <summary>
    /// Implementación simulada de un servicio externo de países.
    /// Devuelve una lista fija en memoria.
    /// </summary>
    public class ExternalCountryService : IExternalCountryService
    {
        public Task<IReadOnlyList<ExternalCountryDto>> GetCountriesAsync()
        {
            // Lista simulada de países (podrías ampliarla o leerla de un JSON local).
            var countries = new List<ExternalCountryDto>
            {
                new ExternalCountryDto { Name = "Colombia",   IsoCode = "CO" },
                new ExternalCountryDto { Name = "Peru",       IsoCode = "PE" },
                new ExternalCountryDto { Name = "Mexico",     IsoCode = "MX" },
                new ExternalCountryDto { Name = "Chile",      IsoCode = "CL" },
                new ExternalCountryDto { Name = "Argentina",  IsoCode = "AR" },
            };

            return Task.FromResult<IReadOnlyList<ExternalCountryDto>>(countries);
        }
    }
}