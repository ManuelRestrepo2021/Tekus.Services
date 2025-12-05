namespace Tekus.Services.Application.Dtos
{
    /// <summary>
    /// DTO que representa la cantidad de servicios distintos ofrecidos en un país.
    /// Se calcula a partir de los proveedores de ese país y sus servicios asociados.
    /// </summary>
    public class ServicesByCountryDto
    {
        public int CountryId { get; set; }

        public string CountryName { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de servicios distintos ofrecidos en este país.
        /// </summary>
        public int ServiceCount { get; set; }
    }
}