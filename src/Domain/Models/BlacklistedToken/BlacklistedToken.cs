namespace TiendaUCN.src.Domain.Models
{
    /// <summary>
    /// Entidad de tokens en lista negra.
    /// </summary>
    public class BlacklistedToken
    {
        /// <summary>
        /// Identificador del token.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador único del token.
        /// </summary>
        public required string TokenId { get; set; }

        /// <summary>
        /// Fecha de expiración del token.
        /// </summary>
        public required DateTime ExpireAt { get; set; }
    }
}