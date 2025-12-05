using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Tekus.Services.Application.Dtos;
using Tekus.Services.Application.Interfaces;


namespace Tekus.Services.Infrastructure.Services
{
    /// <summary>
    /// Implementación del servicio de autenticación usando JWT.
    /// Utiliza un usuario por defecto configurado en appsettings.json.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            // Leer usuario por defecto desde configuración
            var defaultUsername = _configuration["DefaultUser:Username"];
            var defaultPassword = _configuration["DefaultUser:Password"];

            if (string.IsNullOrWhiteSpace(defaultUsername) ||
                string.IsNullOrWhiteSpace(defaultPassword))
            {
                // Configuración inválida, podrías lanzar excepción o registrar log.
                return Task.FromResult<LoginResponseDto?>(null);
            }

            // Validar credenciales (usuario fijo)
            if (!string.Equals(request.Username, defaultUsername, StringComparison.OrdinalIgnoreCase) ||
                request.Password != defaultPassword)
            {
                // Credenciales inválidas
                return Task.FromResult<LoginResponseDto?>(null);
            }

            // Generar token JWT
            var key = _configuration["JwtSettings:Key"]
          ?? throw new InvalidOperationException("JwtSettings:Key is missing");

            var issuer = _configuration["JwtSettings:Issuer"] ?? "Tekus.Services.Api";
            var audience = _configuration["JwtSettings:Audience"] ?? "Tekus.Services.Client";

            var expiresInMinutesString = _configuration["JwtSettings:ExpiresInMinutes"];
            var expiresInMinutes = 60;
            if (!string.IsNullOrWhiteSpace(expiresInMinutesString) &&
                int.TryParse(expiresInMinutesString, out var parsed))
            {
                expiresInMinutes = parsed;
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(expiresInMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, defaultUsername),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, defaultUsername),
                new Claim(ClaimTypes.Role, "Admin") // rol fijo para ejemplo
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new LoginResponseDto
            {
                Token = tokenString,
                ExpiresAt = expires
            };

            return Task.FromResult<LoginResponseDto?>(response);
        }
    }
}