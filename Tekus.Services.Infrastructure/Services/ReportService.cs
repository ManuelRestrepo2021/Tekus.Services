using Microsoft.EntityFrameworkCore;
using Tekus.Services.Application.Dtos;
using Tekus.Services.Application.Interfaces;
using Tekus.Services.Infrastructure.Persistence;

namespace Tekus.Services.Infrastructure.Services
{
    /// <summary>
    /// Implementación del servicio de reportes usando EF Core.
    /// Se encarga de construir indicadores agregados a partir de los datos
    /// de proveedores, países y servicios.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly AppDbContext _dbContext;

        public ReportService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Obtiene un resumen con indicadores globales:
        /// - Cantidad de proveedores por país.
        /// - Cantidad de servicios distintos ofrecidos por país.
        /// </summary>
        public async Task<SummaryReportDto> GetSummaryAsync()
        {
            // ------------------------------------------------
            // Indicador 1: Proveedores por país
            // ------------------------------------------------
            // Agrupamos los proveedores por CountryId y nombre de país,
            // y contamos cuántos proveedores hay en cada grupo.
            var providersByCountry = await _dbContext.Providers
                .Include(p => p.Country)
                .GroupBy(p => new { p.CountryId, CountryName = p.Country!.Name })
                .Select(g => new ProvidersByCountryDto
                {
                    CountryId = g.Key.CountryId,
                    CountryName = g.Key.CountryName,
                    ProviderCount = g.Count()
                })
                .OrderByDescending(x => x.ProviderCount)
                .ToListAsync();

            // ------------------------------------------------
            // Indicador 2: Servicios por país
            // ------------------------------------------------
            // Para cada proveedor tomamos sus servicios, proyectamos una fila
            // (CountryId, CountryName, ServiceId) y luego:
            // - agrupamos por país
            // - contamos la cantidad de servicios DISTINTOS en cada país.
            var servicesByCountry = await _dbContext.Providers
                .Include(p => p.Country)
                .Include(p => p.Services)
                .SelectMany(p => p.Services.Select(s => new
                {
                    p.CountryId,
                    CountryName = p.Country!.Name,
                    ServiceId = s.Id
                }))
                .GroupBy(x => new { x.CountryId, x.CountryName })
                .Select(g => new ServicesByCountryDto
                {
                    CountryId = g.Key.CountryId,
                    CountryName = g.Key.CountryName,
                    ServiceCount = g
                        .Select(x => x.ServiceId)
                        .Distinct()
                        .Count()
                })
                .OrderByDescending(x => x.ServiceCount)
                .ToListAsync();

            // Devolvemos el objeto resumen con ambos indicadores
            return new SummaryReportDto
            {
                ProvidersByCountry = providersByCountry,
                ServicesByCountry = servicesByCountry
            };
        }
    }
}