namespace Tekus.Services.Domain.Entities
{
    /// <summary>
    /// Entidad de dominio que representa un país donde
    /// se pueden ofrecer servicios.
    /// La lista real de países se consultará desde un servicio externo,
    /// pero se persiste aquí para las relaciones con proveedores y servicios.
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Identificador único del país.
        /// Es un valor autogenerado en la base de datos (IDENTITY).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre del país (por ejemplo: "Colombia", "Mexico").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Código ISO del país (por ejemplo: "CO", "MX").
        /// </summary>
        public string IsoCode { get; set; } = string.Empty;
    }
}