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
        /// Obtiene una lista paginada de proveedores, con soporte para búsqueda y ordenamiento.
        /// </summary>
        /// <param name="page">
        /// Número de página (1‑based). Si es menor o igual a 0, se normaliza internamente a 1.
        /// </param>
        /// <param name="pageSize">
        /// Tamaño de página (cantidad de registros por página).
        /// Si es menor o igual a 0, se normaliza internamente a un valor por defecto (por ejemplo 10).
        /// </param>
        /// <param name="search">
        /// Texto de búsqueda opcional. Se aplica típicamente sobre campos como
        /// Nombre, NIT o correo electrónico del proveedor.
        /// </param>
        /// <param name="sortField">
        /// Campo por el cual se ordenará la lista.
        /// Valores esperados: "name", "nit", "email", "country" (o "countryName").
        /// Si es nulo o vacío, se ordena por Id de manera predeterminada.
        /// </param>
        /// <param name="sortDir">
        /// Dirección de ordenamiento: "asc" para ascendente o "desc" para descendente.
        /// Si es nulo o diferente de estos valores, se asume "asc".
        /// </param>
        /// <returns>
        /// Un objeto <see cref="PagedResult{T}"/> que contiene la página de proveedores
        /// (<see cref="ProviderDto"/>) y metadatos de paginación (TotalCount, Page, PageSize).
        /// </returns>
        Task<PagedResult<ProviderDto>> GetAllAsync(
            int page,
            int pageSize,
            string? search,
            string? sortField,
            string? sortDir);

        /// <summary>
        /// Obtiene un proveedor por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del proveedor.</param>
        /// <returns>
        /// Un <see cref="ProviderDto"/> si existe el proveedor;
        /// de lo contrario, <c>null</c>.
        /// </returns>
        Task<ProviderDto?> GetByIdAsync(int id);

        /// <summary>
        /// Crea un nuevo proveedor.
        /// </summary>
        /// <param name="dto">
        /// Datos necesarios para la creación del proveedor
        /// (NIT, nombre, correo, país, servicios y campos personalizados).
        /// </param>
        /// <returns>
        /// El proveedor creado representado como <see cref="ProviderDto"/>.
        /// </returns>
        Task<ProviderDto> CreateAsync(ProviderUpsertDto dto);

        /// <summary>
        /// Actualiza un proveedor existente.
        /// </summary>
        /// <param name="id">Identificador del proveedor a actualizar.</param>
        /// <param name="dto">
        /// Datos actualizados del proveedor
        /// (NIT, nombre, correo, país, servicios y campos personalizados).
        /// </param>
        /// <returns>
        /// El proveedor actualizado como <see cref="ProviderDto"/>,
        /// o <c>null</c> si el proveedor no existe.
        /// </returns>
        Task<ProviderDto?> UpdateAsync(int id, ProviderUpsertDto dto);

        /// <summary>
        /// Elimina un proveedor por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor a eliminar.</param>
        /// <returns>
        /// <c>true</c> si el proveedor fue eliminado correctamente;
        /// <c>false</c> si el proveedor no existe.
        /// </returns>
        Task<bool> DeleteAsync(int id);
    }
}