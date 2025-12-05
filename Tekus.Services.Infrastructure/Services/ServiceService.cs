using Microsoft.EntityFrameworkCore;
using Tekus.Services.Application.Dtos;
using Tekus.Services.Application.Interfaces;
using Tekus.Services.Domain.Entities;
using Tekus.Services.Infrastructure.Persistence;

namespace Tekus.Services.Infrastructure.Services
{
    /// <summary>
    /// Implementación del servicio de aplicación para gestionar servicios.
    /// </summary>
    public class ServiceService : IServiceService
    {
        private readonly AppDbContext _dbContext;

        public ServiceService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Obtiene una lista paginada de servicios, con soporte para búsqueda y ordenamiento.
        /// </summary>
        public async Task<PagedResult<ServiceDto>> GetAllAsync(
            int page,
            int pageSize,
            string? search,
            string? sortField,
            string? sortDir)
        {
            var query = _dbContext.Services.AsQueryable();

            // Búsqueda: por nombre o descripción
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(s =>
                    s.Name.ToLower().Contains(search) ||
                    (s.Description != null && s.Description.ToLower().Contains(search)));
            }

            var totalCount = await query.CountAsync();

            // Ordenamiento
            bool descending = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
            var field = (sortField ?? string.Empty).ToLower();

            query = field switch
            {
                "name" => descending
                    ? query.OrderByDescending(s => s.Name)
                    : query.OrderBy(s => s.Name),

                "hourlyrate" => descending
                    ? query.OrderByDescending(s => s.HourlyRate)
                    : query.OrderBy(s => s.HourlyRate),

                _ => query.OrderBy(s => s.Id)
            };

            // Paginación
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var skip = (page - 1) * pageSize;

            var services = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var items = services.Select(MapToDto).ToList();

            return new PagedResult<ServiceDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<ServiceDto?> GetByIdAsync(int id)
        {
            var service = await _dbContext.Services.FindAsync(id);
            return service is null ? null : MapToDto(service);
        }

        public async Task<ServiceDto> CreateAsync(ServiceUpsertDto dto)
        {
            var service = new Service
            {
                Name = dto.Name,
                Description = dto.Description,
                HourlyRate = dto.HourlyRate
            };

            _dbContext.Services.Add(service);
            await _dbContext.SaveChangesAsync();

            return MapToDto(service);
        }

        public async Task<ServiceDto?> UpdateAsync(int id, ServiceUpsertDto dto)
        {
            var service = await _dbContext.Services.FindAsync(id);
            if (service is null)
            {
                return null;
            }

            service.Name = dto.Name;
            service.Description = dto.Description;
            service.HourlyRate = dto.HourlyRate;

            await _dbContext.SaveChangesAsync();

            return MapToDto(service);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var service = await _dbContext.Services.FindAsync(id);
            if (service is null)
            {
                return false;
            }

            _dbContext.Services.Remove(service);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Mapea una entidad Service a un DTO ServiceDto.
        /// </summary>
        private static ServiceDto MapToDto(Service service) =>
            new()
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                HourlyRate = service.HourlyRate
            };
    }
}