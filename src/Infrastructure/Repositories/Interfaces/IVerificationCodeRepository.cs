using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de códigos de verificación.
    /// </summary>
    public interface IVerificationCodeRepository
    {
        /// <summary>
        /// Crea un nuevo código de verificación.
        /// </summary>
        /// <param name="verificationCode">El código de verificación a crear</param>
        /// <returns>El código de verificación creado</returns>
        Task<VerificationCode> CreateAsync(VerificationCode verificationCode);

        /// <summary>
        /// Actualiza el número de intentos fallidos para un código de verificación.
        /// </summary>
        /// <param name="id">El ID del código de verificación</param>
        /// <returns>true si se actualiza correctamente, false si no</returns>
        Task<bool> UpdateFailedAttemptsAsync(int id);

        /// <summary>
        /// Actualiza un código de verificación.
        /// </summary>
        /// <param name="id">El ID del código de verificación</param>
        /// <param name="code">El nuevo código</param>
        /// <param name="expiry">La fecha de expiración</param>
        /// <returns>true si se actualiza correctamente, false si no</returns>
        Task<bool> UpdateAsync(int id, string code, DateTime expiry);

        /// <summary>
        /// Actualiza la fecha para reenviar el código de verificación.
        /// </summary>
        /// <param name="id">El ID del código de verificación</param>
        /// <param name="dateToResend">La fecha para reenviar el código</param>
        /// <returns>true si se actualiza correctamente, false si no</returns>
        Task<bool> UpdateDateToResendAsync(int id, DateTime dateToResend);
    }
}