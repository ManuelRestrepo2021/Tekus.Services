namespace Tekus.Services.Application.Dtos
{
    /// <summary>
    /// DTO para exponer información de un servicio.
    /// </summary>
    public class ServiceDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Nombre del servicio.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descripción opcional del servicio.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Valor por hora en USD.
        /// </summary>
        public decimal HourlyRate { get; set; }
    }
}