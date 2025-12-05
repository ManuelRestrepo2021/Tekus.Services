using Tekus.Services.Application.Dtos;

namespace Tekus.Services.Application.Interfaces
{
    /// <summary>
    /// Servicio de aplicación para gestionar servicios (Service).
    /// </summary>
    public interface IServiceService
    {
        /// <summary>
        /// Obtiene una lista paginada de servicios, con soporte para búsqueda y ordenamiento.
        /// </summary>
        /// <param name="page">Número de página (1‑based).</param>
        /// <param name="pageSize">Tamaño de página (registros por página).</param>
        /// <param name="search">Texto de búsqueda opcional (por nombre o descripción).</param>
        /// <param name="sortField">Campo de ordenamiento ("name", "hourlyRate").</param>
        /// <param name="sortDir">Dirección de ordenamiento ("asc" o "desc").</param>
        Task<PagedResult<ServiceDto>> GetAllAsync(
            int page,
            int pageSize,
            string? search,
            string? sortField,
            string? sortDir);

        Task<ServiceDto?> GetByIdAsync(int id);
        Task<ServiceDto> CreateAsync(ServiceUpsertDto dto);
        Task<ServiceDto?> UpdateAsync(int id, ServiceUpsertDto dto);
        Task<bool> DeleteAsync(int id);
    }
}