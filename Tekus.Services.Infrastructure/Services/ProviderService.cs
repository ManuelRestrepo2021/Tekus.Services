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

        /// <summary>
        /// Obtiene una lista paginada de proveedores, con soporte para búsqueda y ordenamiento.
        /// </summary>
        /// <param name="page">Número de página (1‑based).</param>
        /// <param name="pageSize">Tamaño de página (registros por página).</param>
        /// <param name="search">Texto de búsqueda opcional (Name, Nit, Email).</param>
        /// <param name="sortField">Campo de ordenamiento ("name", "nit", "email", "country").</param>
        /// <param name="sortDir">Dirección de ordenamiento ("asc" o "desc").</param>
        public async Task<PagedResult<ProviderDto>> GetAllAsync(
            int page,
            int pageSize,
            string? search,
            string? sortField,
            string? sortDir)
        {
            // Query base con las relaciones necesarias
            var query = _dbContext.Providers
                .Include(p => p.Country)
                .Include(p => p.Services)
                .Include(p => p.CustomFields)
                .AsQueryable();

            // ------------------------
            // Filtro de búsqueda
            // ------------------------
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(p =>
                    p.Name.ToLower().Contains(search) ||
                    p.Nit.ToLower().Contains(search) ||
                    p.Email.ToLower().Contains(search));
            }

            // Contar total antes de paginar
            var totalCount = await query.CountAsync();

            // ------------------------
            // Ordenamiento
            // ------------------------
            bool descending = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
            var field = (sortField ?? string.Empty).ToLower();

            query = field switch
            {
                "name" => descending
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),

                "nit" => descending
                    ? query.OrderByDescending(p => p.Nit)
                    : query.OrderBy(p => p.Nit),

                "email" => descending
                    ? query.OrderByDescending(p => p.Email)
                    : query.OrderBy(p => p.Email),

                "country" or "countryname" => descending
                    ? query.OrderByDescending(p => p.Country!.Name)
                    : query.OrderBy(p => p.Country!.Name),

                _ => query.OrderBy(p => p.Id) // Orden por defecto
            };

            // ------------------------
            // Normalizar paginación
            // ------------------------
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var skip = (page - 1) * pageSize;

            // ------------------------
            // Ejecutar query paginada
            // ------------------------
            var providers = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var items = providers.Select(MapToDto).ToList();

            return new PagedResult<ProviderDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// Obtiene un proveedor por su identificador.
        /// </summary>
        public async Task<ProviderDto?> GetByIdAsync(int id)
        {
            var provider = await _dbContext.Providers
                .Include(p => p.Country)
                .Include(p => p.Services)
                .Include(p => p.CustomFields)
                .FirstOrDefaultAsync(p => p.Id == id);

            return provider is null ? null : MapToDto(provider);
        }

        /// <summary>
        /// Crea un nuevo proveedor.
        /// </summary>
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

        /// <summary>
        /// Actualiza un proveedor existente.
        /// </summary>
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

        /// <summary>
        /// Elimina un proveedor por su identificador.
        /// </summary>
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