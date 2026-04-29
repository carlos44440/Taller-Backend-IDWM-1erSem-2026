using Microsoft.EntityFrameworkCore;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Infrastructure.Repositories.Implements
{
    /// <summary>
    /// Repositorio del codigo de verificacion.
    /// </summary>
    public class VerificationCodeRepository : IVerificationCodeRepository
    {
        /// <summary>
        /// Contexto de la base de datos.
        /// </summary>
        private readonly DataContext _context;

        /// <summary>
        /// Constructor del repositorio de códigos de verificación.
        /// </summary>
        /// <param name="context">El contexto de la base de datos</param>
        public VerificationCodeRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Crea un nuevo código de verificación.
        /// </summary>
        /// <param name="verificationCode">El código de verificación a crear</param>
        /// <returns>El código de verificación creado</returns>
        public async Task<VerificationCode> CreateAsync(VerificationCode verificationCode)
        {
            var result = await _context.VerificationCodes.AddAsync(verificationCode);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Actualiza el número de intentos fallidos para un código de verificación.
        /// </summary>
        /// <param name="id">El ID del código de verificación</param>
        /// <returns>true si se actualiza correctamente, false si no</returns>
        public async Task<bool> UpdateFailedAttemptsAsync(int id)
        {
            // Incrementa el contador de intentos fallidos para el código de verificación con el ID especificado
            var result = await _context.VerificationCodes
                .Where(v => v.Id == id)
               .ExecuteUpdateAsync(v => v.SetProperty(x => x.FailedAttempts, x => x.FailedAttempts + 1));
            return result > 0;
        }

        /// <summary>
        /// Actualiza un código de verificación.
        /// </summary>
        /// <param name="id">El ID del código de verificación</param>
        /// <param name="code">El nuevo código</param>
        /// <param name="expiry">La fecha de expiración</param>
        /// <returns>true si se actualiza correctamente, false si no</returns>
        public async Task<bool> UpdateAsync(int id, string code, DateTime expiry)
        {
            // Actualiza el código de verificación y su fecha de expiración para el código con el ID especificado
            var result = await _context.VerificationCodes
                .Where(v => v.Id == id)
                .ExecuteUpdateAsync(v => v.SetProperty(x => x.Code, code).SetProperty(x => x.Expiry, expiry));
            return result > 0;
        }

        /// <summary>
        /// Actualiza la fecha para reenviar el código de verificación.
        /// </summary>
        /// <param name="id">El ID del código de verificación</param>
        /// <param name="dateToResend">La fecha para reenviar el código</param>
        /// <returns>true si se actualiza correctamente, false si no</returns>
        public async Task<bool> UpdateDateToResendAsync(int id, DateTime dateToResend)
        {
            // Actualiza la fecha para reenviar un nuevo código de verificación para el código con el ID especificado
            var result = await _context.VerificationCodes.Where(v => v.Id == id)
                .ExecuteUpdateAsync(v => v.SetProperty(x => x.DateToResend, dateToResend));
            return result > 0;
        }
    }
}