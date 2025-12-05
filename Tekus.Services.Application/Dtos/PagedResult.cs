namespace Tekus.Services.Application.Dtos
{
    /// <summary>
    /// Resultado paginado genérico para listados.
    /// </summary>
    /// <typeparam name="T">Tipo de elemento contenido en la página.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Elementos de la página actual.
        /// </summary>
        public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();

        /// <summary>
        /// Cantidad total de registros que cumplen el filtro.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Número de página actual (1-based).
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Tamaño de página (cantidad de registros por página).
        /// </summary>
        public int PageSize { get; set; }
    }
}