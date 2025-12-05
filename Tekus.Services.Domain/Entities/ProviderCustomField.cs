namespace Tekus.Services.Domain.Entities
{
    /// <summary>
    /// Entidad de dominio que representa un campo personalizado
    /// definido para un proveedor específico.
    /// 
    /// Ejemplos de campos personalizados:
    /// - "Número de contacto en Marte"
    /// - "Cantidad de mascotas en la nómina"
    /// </summary>
    public class ProviderCustomField
    {
        /// <summary>
        /// Identificador único del campo personalizado.
        /// Es un valor autogenerado en la base de datos (IDENTITY).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador del proveedor al que pertenece este campo.
        /// </summary>
        public int ProviderId { get; set; }

        /// <summary>
        /// Referencia de navegación al proveedor dueño del campo.
        /// </summary>
        public Provider? Provider { get; set; }

        /// <summary>
        /// Nombre del campo personalizado
        /// (por ejemplo: "Número de contacto en Marte").
        /// </summary>
        public string FieldName { get; set; } = string.Empty;

        /// <summary>
        /// Valor del campo personalizado
        /// (por ejemplo: "+99 123 456 789").
        /// </summary>
        public string FieldValue { get; set; } = string.Empty;
    }
}