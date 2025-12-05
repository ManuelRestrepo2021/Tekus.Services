namespace Tekus.Services.Application.Dtos
{
    /// <summary>
    /// DTO para crear o actualizar un país.
    /// (En la prueba real podrías poblarlos desde un servicio externo).
    /// </summary>
    public class CountryUpsertDto
    {
        public string Name { get; set; } = string.Empty;

        public string IsoCode { get; set; } = string.Empty;
    }
}