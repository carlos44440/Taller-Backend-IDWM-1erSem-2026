using System.ComponentModel.DataAnnotations;

namespace TiendaUCN.src.Application.DTOs.AuthDTO
{
    /// <summary>
    /// DTO para la verificación de correo electrónico.
    /// </summary>
    public class EmailVerificationDTO
    {
        /// <summary>
        /// Correo electrónico del usuario que se va a verificar.
        /// </summary>
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        public required string Email { get; set; }

        /// <summary>
        /// Código de verificación enviado al correo electrónico.
        /// </summary>
        [Required(ErrorMessage = "El código de verificación es obligatorio.")]
        public required string VerificationCode { get; set; }
    }
}