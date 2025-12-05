namespace Tekus.Services.Domain.Entities
{
    /// <summary>
    /// Entidad de dominio que representa un proveedor de servicios de Tekus.
    /// Un proveedor ofrece uno o varios servicios y está asociado a un país.
    /// Además, puede tener campos personalizados definidos por el usuario.
    /// </summary>
    public class Provider
    {
        /// <summary>
        /// Identificador único del proveedor.
        /// Es un valor autogenerado en la base de datos (IDENTITY).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Número de identificación tributaria del proveedor (NIT).
        /// Este campo es obligatorio según el enunciado de la prueba.
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
        /// Identificador del país principal asociado al proveedor.
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Referencia de navegación al país principal del proveedor.
        /// </summary>
        public Country? Country { get; set; }

        /// <summary>
        /// Lista de servicios que este proveedor ofrece.
        /// Relación muchos a muchos entre Provider y Service.
        /// </summary>
        public ICollection<Service> Services { get; set; } = new List<Service>();

        /// <summary>
        /// Campos personalizados definidos específicamente para este proveedor.
        /// Por ejemplo: "Número de contacto en Marte", "Cantidad de mascotas en la nómina".
        /// </summary>
        public ICollection<ProviderCustomField> CustomFields { get; set; } = new List<ProviderCustomField>();
    }
}