namespace TiendaUCN.src.Domain.Models
{
    /// <summary>
    /// Entidad de código de verificación.
    /// </summary>
    public class VerificationCode
    {
        /// <summary>
        /// Identificador del código.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Código de verificación.
        /// </summary>
        public required string Code { get; set; }

        /// <summary>
        /// Fecha de expiración del código.
        /// </summary>
        public required DateTime Expiry { get; set; }

        /// <summary>
        /// Número de intentos fallidos.
        /// </summary>
        public int FailedAttempts { get; set; } = 0;

        /// <summary>
        /// Fecha para reenviar el código.
        /// </summary>
        public DateTime DateToResend { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int UserId { get; set; }
    }
}