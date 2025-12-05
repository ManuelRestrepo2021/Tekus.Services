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

        public async Task<IReadOnlyList<ServiceDto>> GetAllAsync()
        {
            var services = await _dbContext.Services.AsNoTracking().ToListAsync();
            return services.Select(MapToDto).ToList();
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