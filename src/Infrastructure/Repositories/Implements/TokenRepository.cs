using Microsoft.EntityFrameworkCore;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Infrastructure.Repositories.Implements
{
    /// <summary>
    /// Repositorio de tokens.
    /// </summary>
    public class TokenRepository : ITokenRepository
    {
        /// <summary>
        /// Contexto de datos para acceder a la base de datos.
        /// </summary>
        private readonly DataContext _context;

        /// <summary>
        /// Constructor del repositorio de tokens.
        /// </summary>
        /// <param name="context">El contexto de datos</param>
        public TokenRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Agrega un token a la lista negra.
        /// </summary>
        /// <param name="token">El token a agregar</param>
        /// <returns></returns>
        public async Task AddAsync(BlacklistedToken token)
        {
            await _context.BlacklistedTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Verifica si un token está en la lista negra.
        /// </summary>
        /// <param name="tokenId">El ID del token</param>
        /// <returns>true si está en la lista negra, false si no</returns>
        public async Task<bool> IsBlacklistedAsync(string tokenId)
        {
            return await _context.BlacklistedTokens.AnyAsync(u => u.TokenId == tokenId);
        }

        /// <summary>
        /// Elimina los tokens expirados de la lista negra.
        /// </summary>
        /// <returns>El número de tokens eliminados</returns>
        public async Task<int> DeleteExpiredTokensAsync()
        {
            // Elimina los tokens que han expirado, sin soft delete
            var now = DateTime.UtcNow;
            return await _context.BlacklistedTokens
                .Where(t => t.ExpireAt < now)
                .ExecuteDeleteAsync();
        }
    }
}