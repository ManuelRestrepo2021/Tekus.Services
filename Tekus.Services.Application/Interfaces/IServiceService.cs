using Tekus.Services.Application.Dtos;

namespace Tekus.Services.Application.Interfaces
{
    /// <summary>
    /// Servicio de aplicación para gestionar servicios (Service).
    /// </summary>
    public interface IServiceService
    {
        Task<IReadOnlyList<ServiceDto>> GetAllAsync();
        Task<ServiceDto?> GetByIdAsync(int id);
        Task<ServiceDto> CreateAsync(ServiceUpsertDto dto);
        Task<ServiceDto?> UpdateAsync(int id, ServiceUpsertDto dto);
        Task<bool> DeleteAsync(int id);
    }
}