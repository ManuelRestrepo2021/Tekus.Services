using System.Collections.Generic;
using System.Threading.Tasks;
using Tekus.Services.Application.Dtos;

namespace Tekus.Services.Application.Interfaces
{
    /// <summary>
    /// Servicio de aplicación para gestionar proveedores.
    /// Contiene la lógica de los casos de uso relacionados con Providers.
    /// </summary>
    public interface IProviderService
    {
        /// <summary>
        /// Obtiene la lista de todos los proveedores registrados.
        /// </summary>
        Task<IReadOnlyList<ProviderDto>> GetAllAsync();

        /// <summary>
        /// Obtiene un proveedor por su identificador.
        /// </summary>
        Task<ProviderDto> GetByIdAsync(int id);

        /// <summary>
        /// Crea un nuevo proveedor.
        /// </summary>
        Task<ProviderDto> CreateAsync(ProviderUpsertDto dto);

        /// <summary>
        /// Actualiza un proveedor existente.
        /// </summary>
        Task<ProviderDto> UpdateAsync(int id, ProviderUpsertDto dto);

        /// <summary>
        /// Elimina un proveedor por su identificador.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}