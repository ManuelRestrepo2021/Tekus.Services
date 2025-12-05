namespace Tekus.Services.Application.Dtos
{
    /// <summary>
    /// DTO para exponer información de un país.
    /// </summary>
    public class CountryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string IsoCode { get; set; } = string.Empty;
    }
}