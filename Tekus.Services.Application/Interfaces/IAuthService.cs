using System.Threading.Tasks;
using Tekus.Services.Application.Dtos;

namespace Tekus.Services.Application.Interfaces
{
    /// <summary>
    /// Servicio de aplicación para autenticación de usuarios y generación de tokens JWT.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Valida las credenciales y genera un token JWT si son correctas.
        /// </summary>
        /// <param name="request">Credenciales de inicio de sesión.</param>
        /// <returns>
        /// Respuesta con token JWT si las credenciales son válidas;
        /// null si el usuario o la contraseña no son correctos.
        /// </returns>
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    }
}