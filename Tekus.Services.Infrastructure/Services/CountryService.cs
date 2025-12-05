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

        /// <summary>
        /// Obtiene una lista paginada de países, con soporte para búsqueda y ordenamiento.
        /// </summary>
        public async Task<PagedResult<CountryDto>> GetAllAsync(
            int page,
            int pageSize,
            string? search,
            string? sortField,
            string? sortDir)
        {
            var query = _dbContext.Countries.AsQueryable();

            // Búsqueda: por nombre o IsoCode
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(search) ||
                    c.IsoCode.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync();

            // Ordenamiento
            bool descending = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
            var field = (sortField ?? string.Empty).ToLower();

            query = field switch
            {
                "name" => descending
                    ? query.OrderByDescending(c => c.Name)
                    : query.OrderBy(c => c.Name),

                "isocode" => descending
                    ? query.OrderByDescending(c => c.IsoCode)
                    : query.OrderBy(c => c.IsoCode),

                _ => query.OrderBy(c => c.Id)
            };

            // Paginación
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var skip = (page - 1) * pageSize;

            var countries = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var items = countries.Select(MapToDto).ToList();

            return new PagedResult<CountryDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
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

        /// <summary>
        /// Mapea una entidad Country a un DTO CountryDto.
        /// </summary>
        private static CountryDto MapToDto(Country country) =>
            new()
            {
                Id = country.Id,
                Name = country.Name,
                IsoCode = country.IsoCode
            };
    }
}