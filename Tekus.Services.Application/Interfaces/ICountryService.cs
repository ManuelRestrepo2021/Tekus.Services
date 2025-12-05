using Tekus.Services.Application.Dtos;

namespace Tekus.Services.Application.Interfaces
{
    /// <summary>
    /// Servicio de aplicación para gestionar países (Country).
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Obtiene una lista paginada de países, con soporte para búsqueda y ordenamiento.
        /// </summary>
        /// <param name="page">Número de página (1‑based).</param>
        /// <param name="pageSize">Tamaño de página (registros por página).</param>
        /// <param name="search">Texto de búsqueda opcional (por nombre o código ISO).</param>
        /// <param name="sortField">Campo de ordenamiento ("name", "isoCode").</param>
        /// <param name="sortDir">Dirección de ordenamiento ("asc" o "desc").</param>
        Task<PagedResult<CountryDto>> GetAllAsync(
            int page,
            int pageSize,
            string? search,
            string? sortField,
            string? sortDir);

        Task<CountryDto?> GetByIdAsync(int id);
        Task<CountryDto> CreateAsync(CountryUpsertDto dto);
        Task<CountryDto?> UpdateAsync(int id, CountryUpsertDto dto);
        Task<bool> DeleteAsync(int id);
    }
}