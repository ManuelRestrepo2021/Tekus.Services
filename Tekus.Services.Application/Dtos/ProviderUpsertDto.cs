namespace Tekus.Services.Application.Dtos
{
    /// <summary>
    /// DTO para crear o actualizar un proveedor.
    /// No incluye el Id, ya que es autogenerado por la base de datos.
    /// </summary>
    public class ProviderUpsertDto
    {
        /// <summary>
        /// Número de identificación tributaria del proveedor (NIT).
        /// </summary>
        public string Nit { get; set; } = string.Empty;

        /// <summary>
        /// Nombre comercial o razón social del proveedor.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico principal de contacto del proveedor.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Número de teléfono principal del proveedor.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Identificador del país principal del proveedor.
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Lista de identificadores de servicios que ofrece el proveedor.
        /// </summary>
        public List<int> ServiceIds { get; set; } = new List<int>();

        /// <summary>
        /// Campos personalizados del proveedor como pares clave/valor.
        /// Ejemplo: "NumeroContactoMarte" -> "+99 123 456 789"
        /// </summary>
        public Dictionary<string, string> CustomFields { get; set; } = new();
    }
}