namespace Tekus.Services.Application.Dtos
{
    /// <summary>
    /// DTO para crear o actualizar un servicio.
    /// </summary>
    public class ServiceUpsertDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal HourlyRate { get; set; }
    }
}