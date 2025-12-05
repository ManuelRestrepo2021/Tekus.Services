using Microsoft.AspNetCore.Mvc;
using Tekus.Services.Application.Dtos;
using Tekus.Services.Application.Interfaces;

namespace Tekus.Services.Api.Controllers
{
    /// <summary>
    /// Controlador responsable de la autenticación de usuarios
    /// y la emisión de tokens JWT.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Inicia sesión con las credenciales proporcionadas y devuelve un token JWT.
        /// </summary>
        /// <param name="request">Credenciales de inicio de sesión (usuario y contraseña).</param>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseDto>> LoginAsync([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);

            if (result is null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(result);
        }
    }
}