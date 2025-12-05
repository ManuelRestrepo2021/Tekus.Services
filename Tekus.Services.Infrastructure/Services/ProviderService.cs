using Microsoft.EntityFrameworkCore;
using Tekus.Services.Application.Dtos;
using Tekus.Services.Application.Interfaces;
using Tekus.Services.Domain.Entities;
using Tekus.Services.Infrastructure.Persistence;

namespace Tekus.Services.Infrastructure.Services
{
    /// <summary>
    /// Implementación del servicio de aplicación para gestionar proveedores.
    /// Usa EF Core y AppDbContext para acceder a la base de datos.
    /// </summary>
    public class ProviderService : IProviderService
    {
        private readonly AppDbContext _dbContext;

        public ProviderService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<ProviderDto>> GetAllAsync()
        {
            var providers = await _dbContext.Providers
                .Include(p => p.Country)
                .Include(p => p.Services)
                .Include(p => p.CustomFields)
                .ToListAsync();

            return providers.Select(MapToDto).ToList();
        }

        public async Task<ProviderDto?> GetByIdAsync(int id)
        {
            var provider = await _dbContext.Providers
                .Include(p => p.Country)
                .Include(p => p.Services)
                .Include(p => p.CustomFields)
                .FirstOrDefaultAsync(p => p.Id == id);

            return provider is null ? null : MapToDto(provider);
        }

        public async Task<ProviderDto> CreateAsync(ProviderUpsertDto dto)
        {
            var provider = new Provider
            {
                Nit = dto.Nit,
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                CountryId = dto.CountryId
            };

            // Servicios asociados
            if (dto.ServiceIds.Any())
            {
                var services = await _dbContext.Services
                    .Where(s => dto.ServiceIds.Contains(s.Id))
                    .ToListAsync();

                provider.Services = services;
            }

            // Campos personalizados
            if (dto.CustomFields?.Any() == true)
            {
                provider.CustomFields = dto.CustomFields
                    .Select(cf => new ProviderCustomField
                    {
                        FieldName = cf.Key,
                        FieldValue = cf.Value
                    })
                    .ToList();
            }

            _dbContext.Providers.Add(provider);
            await _dbContext.SaveChangesAsync();

            // Recargar navegación
            await _dbContext.Entry(provider).Reference(p => p.Country).LoadAsync();
            await _dbContext.Entry(provider).Collection(p => p.Services).LoadAsync();
            await _dbContext.Entry(provider).Collection(p => p.CustomFields).LoadAsync();

            return MapToDto(provider);
        }

        public async Task<ProviderDto?> UpdateAsync(int id, ProviderUpsertDto dto)
        {
            var provider = await _dbContext.Providers
                .Include(p => p.Services)
                .Include(p => p.CustomFields)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (provider is null)
            {
                return null;
            }

            provider.Nit = dto.Nit;
            provider.Name = dto.Name;
            provider.Email = dto.Email;
            provider.PhoneNumber = dto.PhoneNumber;
            provider.CountryId = dto.CountryId;

            // Actualizar servicios
            provider.Services.Clear();
            if (dto.ServiceIds.Any())
            {
                var services = await _dbContext.Services
                    .Where(s => dto.ServiceIds.Contains(s.Id))
                    .ToListAsync();

                foreach (var service in services)
                {
                    provider.Services.Add(service);
                }
            }

            // Actualizar campos personalizados
            provider.CustomFields.Clear();
            if (dto.CustomFields?.Any() == true)
            {
                foreach (var kv in dto.CustomFields)
                {
                    provider.CustomFields.Add(new ProviderCustomField
                    {
                        FieldName = kv.Key,
                        FieldValue = kv.Value
                    });
                }
            }

            await _dbContext.SaveChangesAsync();

            // Recargar navegación
            await _dbContext.Entry(provider).Reference(p => p.Country).LoadAsync();
            await _dbContext.Entry(provider).Collection(p => p.Services).LoadAsync();
            await _dbContext.Entry(provider).Collection(p => p.CustomFields).LoadAsync();

            return MapToDto(provider);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var provider = await _dbContext.Providers.FindAsync(id);
            if (provider is null)
            {
                return false;
            }

            _dbContext.Providers.Remove(provider);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Mapea una entidad Provider a un DTO ProviderDto.
        /// </summary>
        private static ProviderDto MapToDto(Provider provider)
        {
            return new ProviderDto
            {
                Id = provider.Id,
                Nit = provider.Nit,
                Name = provider.Name,
                Email = provider.Email,
                PhoneNumber = provider.PhoneNumber,
                CountryId = provider.CountryId,
                CountryName = provider.Country?.Name ?? string.Empty,
                ServiceIds = provider.Services.Select(s => s.Id).ToList(),
                CustomFields = provider.CustomFields
                    .ToDictionary(cf => cf.FieldName, cf => cf.FieldValue)
            };
        }
    }
}