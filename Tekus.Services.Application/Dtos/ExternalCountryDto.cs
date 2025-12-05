namespace Tekus.Services.Application.Dtos
{
    /// <summary>
    /// DTO que representa un país proveniente de un servicio externo.
    /// </summary>
    public class ExternalCountryDto
    {
        /// <summary>
        /// Nombre del país.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Código ISO del país (por ejemplo, CO, PE, MX).
        /// </summary>
        public string IsoCode { get; set; } = string.Empty;
    }
}