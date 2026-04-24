using System.ComponentModel.DataAnnotations;

namespace TiendaUCN.src.Application.DTOs.AuthDTO
{
    /// <summary>
    /// DTO para el inicio de sesión de un usuario.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        public required string Email { get; set; }

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public required string Password { get; set; }
    }
}