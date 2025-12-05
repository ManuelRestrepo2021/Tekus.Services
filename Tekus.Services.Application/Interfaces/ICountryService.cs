using Tekus.Services.Application.Dtos;

namespace Tekus.Services.Application.Interfaces
{
    /// <summary>
    /// Servicio de aplicación para gestionar países (Country).
    /// </summary>
    public interface ICountryService
    {
        Task<IReadOnlyList<CountryDto>> GetAllAsync();
        Task<CountryDto?> GetByIdAsync(int id);
        Task<CountryDto> CreateAsync(CountryUpsertDto dto);
        Task<CountryDto?> UpdateAsync(int id, CountryUpsertDto dto);
        Task<bool> DeleteAsync(int id);
    }
}