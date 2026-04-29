using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TiendaUCN.src.Application.Services.Interfaces;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Application.Services.Implements
{
    /// <summary>
    /// Servicio de gestión de tokens JWT.
    /// </summary>
    public class TokenService : ITokenService
    {
        /// <summary>
        /// Clave secreta.
        /// </summary>
        private readonly string _jwtSecret;

        /// <summary>
        /// Interfaz del repositorio de tokens.
        /// </summary>
        private readonly ITokenRepository _tokenRepository;

        /// <summary>
        /// Interfaz de configuración.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Tiempo de expiración del token en horas.
        /// </summary>
        private readonly int _tokenExpirationInHours;

        /// <summary>
        /// Constructor del servicio de tokens. 
        /// </summary>
        /// <param name="tokenRepository">Interfaz del repositorio de tokens</param>
        /// <param name="configuration">Interfaz de configuración</param>
        /// <exception cref="InvalidOperationException"></exception>
        public TokenService(ITokenRepository tokenRepository, IConfiguration configuration)
        {
            _tokenRepository = tokenRepository;
            _configuration = configuration;
            _tokenExpirationInHours = int.Parse(_configuration["Token:ExpirationTimeInHours"] ?? throw new InvalidOperationException("Token expiration time is not configured."));
            _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("JWT secret key is not configured.");
        }

        /// <summary>
        /// Genera un token JWT.
        /// </summary>
        /// <param name="user">El usuario para el cual generar el token</param>
        /// <param name="roleName">El nombre del rol del usuario</param>
        /// <returns>El token JWT generado</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public string GenerateToken(User user, string roleName)
        {
            try
            {
                // Listamos los claims que queremos incluir en el token (solo las necesarias, no todas las propiedades del usuario)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Role, roleName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Reclamación del ID único del token
                };

                // Creamos la clave de seguridad
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSecret));

                // Creamos las credenciales de firma, ojo la clave debe ser lo suficientemente larga y segura (256 bits mínimo para HMACSHA256) que son 32 caracteres
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Creamos el token
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(_tokenExpirationInHours), // El token expirará en el tiempo configurado
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

        /// <summary>
        /// Agrega un token a la lista negra (blacklist).
        /// </summary>
        /// <param name="token">El token a agregar a la lista negra</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task AddToBlacklistAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Extrae el jti qeu representa el ID único del token y la fecha de expiración
            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value
                ?? throw new InvalidOperationException("El token no contiene un jti válido para agregar a la blacklist");
            var expireAt = jwtToken.ValidTo;

            // Verifica si el token ya está en la blacklist antes de agregarlo
            var isBlacklisted = await _tokenRepository.IsBlacklistedAsync(jti);
            if (isBlacklisted)
            {
                Log.Warning("Intento de agregar a blacklist un token que ya está en la blacklist: {TokenId}", jti);
                throw new InvalidOperationException("El token ya está en la blacklist.");
            }

            // Mappea el token a un modelo de blacklist
            var blacklistedToken = new BlacklistedToken
            {
                TokenId = jti,
                ExpireAt = expireAt
            };

            // Almacena en la blacklist
            await _tokenRepository.AddAsync(blacklistedToken);
        }

        /// <summary>
        /// Verifica si un token está en la lista negra (blacklist).
        /// </summary>
        /// <param name="token">El token a verificar</param>
        /// <returns>True si el token está en la lista negra, false en caso contrario</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            // Lee el token JWT para extraer el jti
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Extrae el jti del token
            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            // Verifica si el jti está en la blacklist
            if (jti != null)
            {
                var isBlacklisted = await _tokenRepository.IsBlacklistedAsync(jti);
                return isBlacklisted;
            }

            Log.Warning("El token no contiene un jti válido para verificar en la blacklist");
            throw new InvalidOperationException("El token no contiene un jti válido.");
        }

        /// <summary>
        /// Elimina los tokens expirados de la lista negra (blacklist).
        /// </summary>
        /// <returns>El número de tokens expirados eliminados</returns>
        public async Task<int> DeleteExpiredTokensInBlacklistAsync()
        {
            int deletedCount = await _tokenRepository.DeleteExpiredTokensAsync();
            Log.Information("Tokens expirados eliminados de la blacklist: {DeletedCount}", deletedCount);
            return deletedCount;
        }
    }
}