namespace Tekus.Services.Application.Dtos
{
    /// <summary>
    /// DTO que representa la cantidad de proveedores agrupados por país.
    /// </summary>
    public class ProvidersByCountryDto
    {
        public int CountryId { get; set; }

        public string CountryName { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de proveedores registrados en este país.
        /// </summary>
        public int ProviderCount { get; set; }
    }
}