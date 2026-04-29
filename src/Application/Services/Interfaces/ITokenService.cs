using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Application.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de gestión de tokens JWT.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Genera un token JWT.
        /// </summary>
        /// <param name="user">El usuario para el cual generar el token</param>
        /// <param name="roleName">El nombre del rol del usuario</param>
        /// <returns>El token JWT generado</returns>
        string GenerateToken(User user, string roleName);

        /// <summary>
        /// Agrega un token a la lista negra (blacklist).
        /// </summary>
        /// <param name="token">El token a agregar</param>
        /// <returns></returns>
        Task AddToBlacklistAsync(string token);

        /// <summary>
        /// Verifica si un token está en la lista negra (blacklist).
        /// </summary>
        /// <param name="token">El token a verificar</param>
        /// <returns>True si el token está en la lista negra, false en caso contrario</returns>
        Task<bool> IsTokenBlacklistedAsync(string token);

        /// <summary>
        /// Elimina los tokens expirados de la lista negra (blacklist).
        /// </summary>
        /// <returns>El número de tokens eliminados</returns>
        Task<int> DeleteExpiredTokensInBlacklistAsync();
    }
}