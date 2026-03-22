using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TiendaUCN.src.Application.Services.Interfaces;
using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Application.Services.Implements
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtSecret;

        public TokenService()
        {
            _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("JWT secret key is not configured.");
        }
        public string GenerateToken(User user, string roleName)
        {
            try
            {
                // Listamos los claims que queremos incluir en el token (solo las necesarias, no todas las propiedades del usuario)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Role, roleName)
                };

                // Creamos la clave de seguridad
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSecret));

                // Creamos las credenciales de firma, ojo la clave debe ser lo suficientemente larga y segura (256 bits mínimo para HMACSHA256) que son 32 caracteres
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Creamos el token
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(24), // El token expira en 24 horas
                    signingCredentials: creds
                );

                // Serializamos el token a string
                Log.Information("Token JWT generado exitosamente para el usuario {UserId}", user.Id);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al generar el token JWT para el usuario {UserId}", user.Id);
                throw new InvalidOperationException("Error al generar el token JWT", ex);
            }
        }
    }
}