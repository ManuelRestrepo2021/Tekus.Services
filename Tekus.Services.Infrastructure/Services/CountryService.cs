using Microsoft.EntityFrameworkCore;
using Tekus.Services.Application.Dtos;
using Tekus.Services.Application.Interfaces;
using Tekus.Services.Domain.Entities;
using Tekus.Services.Infrastructure.Persistence;

namespace Tekus.Services.Infrastructure.Services
{
    /// <summary>
    /// Implementación del servicio de aplicación para gestionar países.
    /// </summary>
    public class CountryService : ICountryService
    {
        private readonly AppDbContext _dbContext;

        public CountryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<CountryDto>> GetAllAsync()
        {
            var countries = await _dbContext.Countries.AsNoTracking().ToListAsync();
            return countries.Select(MapToDto).ToList();
        }

        public async Task<CountryDto?> GetByIdAsync(int id)
        {
            var country = await _dbContext.Countries.FindAsync(id);
            return country is null ? null : MapToDto(country);
        }

        public async Task<CountryDto> CreateAsync(CountryUpsertDto dto)
        {
            var country = new Country
            {
                Name = dto.Name,
                IsoCode = dto.IsoCode
            };

            _dbContext.Countries.Add(country);
            await _dbContext.SaveChangesAsync();

            return MapToDto(country);
        }

        public async Task<CountryDto?> UpdateAsync(int id, CountryUpsertDto dto)
        {
            var country = await _dbContext.Countries.FindAsync(id);
            if (country is null)
            {
                return null;
            }

            country.Name = dto.Name;
            country.IsoCode = dto.IsoCode;

            await _dbContext.SaveChangesAsync();

            return MapToDto(country);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var country = await _dbContext.Countries.FindAsync(id);
            if (country is null)
            {
                return false;
            }

            _dbContext.Countries.Remove(country);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private static CountryDto MapToDto(Country country) =>
            new()
            {
                Id = country.Id,
                Name = country.Name,
                IsoCode = country.IsoCode
            };
    }
}