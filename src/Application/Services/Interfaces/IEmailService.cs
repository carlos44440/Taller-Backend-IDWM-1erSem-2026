namespace TiendaUCN.src.Application.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de correo electrónico.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Envía un correo electrónico con un código de verificación.
        /// </summary>
        /// <param name="email">La dirección de correo electrónico del destinatario.</param>
        /// <param name="verificationCode">El código de verificación a enviar.</param>
        /// <returns></returns>
        Task SendVerificationCodeEmailAsync(string email, string verificationCode);

        /// <summary>
        /// Envía un correo electrónico de bienvenida a un nuevo usuario.
        /// </summary>
        /// <param name="email">La dirección de correo electrónico del destinatario.</param>
        /// <returns></returns>
        Task SendWelcomeEmailAsync(string email);
    }
}