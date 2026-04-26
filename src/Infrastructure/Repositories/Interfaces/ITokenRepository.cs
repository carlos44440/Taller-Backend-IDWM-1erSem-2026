using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de tokens.
    /// </summary>
    public interface ITokenRepository
    {
        /// <summary>
        /// Agrega un token a la lista negra.
        /// </summary>
        /// <param name="token">El token a agregar</param>
        /// <returns></returns>
        Task AddAsync(BlacklistedToken token);

        /// <summary>
        /// Verifica si un token está en la lista negra.
        /// </summary>
        /// <param name="tokenId">El ID del token</param>
        /// <returns>true si está en la lista negra, false si no</returns>
        Task<bool> IsBlacklistedAsync(string tokenId);

        /// <summary>
        /// Elimina los tokens expirados de la lista negra.
        /// </summary>
        /// <returns>El número de tokens eliminados</returns>
        Task<int> DeleteExpiredTokensAsync();
    }
}