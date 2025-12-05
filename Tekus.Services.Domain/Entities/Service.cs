namespace Tekus.Services.Domain.Entities
{
    /// <summary>
    /// Entidad de dominio que representa un tipo de servicio
    /// que ofrecen los proveedores de Tekus.
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Identificador único del servicio.
        /// Es un valor autogenerado en la base de datos (IDENTITY).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre del servicio (por ejemplo:
        /// "Descarga espacial de contenidos",
        /// "Desaparición forzada de bytes").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descripción opcional del servicio.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Valor por hora del servicio en dólares estadounidenses (USD).
        /// Este campo es obligatorio según el enunciado de la prueba.
        /// </summary>
        public decimal HourlyRate { get; set; }
    }
}