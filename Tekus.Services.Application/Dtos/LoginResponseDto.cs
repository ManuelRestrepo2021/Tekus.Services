namespace Tekus.Services.Application.Dtos
{
    /// <summary>
    /// DTO de respuesta al iniciar sesión.
    /// Contiene el token JWT emitido por el sistema.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Token JWT que deberá ser enviado en el header Authorization: Bearer {token}.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de expiración del token (UTC).
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}