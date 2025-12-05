namespace Tekus.Services.Application.Dtos
{
    /// <summary>
    /// DTO principal para el resumen de indicadores del sistema.
    /// </summary>
    public class SummaryReportDto
    {
        /// <summary>
        /// Indicador: cantidad de proveedores por país.
        /// </summary>
        public IReadOnlyList<ProvidersByCountryDto> ProvidersByCountry { get; set; }
            = Array.Empty<ProvidersByCountryDto>();

        /// <summary>
        /// Indicador: cantidad de servicios distintos ofrecidos por país.
        /// </summary>
        public IReadOnlyList<ServicesByCountryDto> ServicesByCountry { get; set; }
            = Array.Empty<ServicesByCountryDto>();
    }
}