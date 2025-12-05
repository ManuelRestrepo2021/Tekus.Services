namespace Tekus.Services.Application.Dtos
{
    /// <summary>
    /// DTO que representa las credenciales de inicio de sesión.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Nombre de usuario para autenticación.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}