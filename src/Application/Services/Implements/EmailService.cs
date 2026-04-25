using Resend;
using TiendaUCN.src.Application.Services.Interfaces;

namespace TiendaUCN.src.Application.Services.Implements
{
    /// <summary>
    /// Servicio de correo electrónico.
    /// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        /// Cliente de Resend.
        /// </summary>
        private readonly IResend _resend;

        /// <summary>
        /// Interfaz de configuración.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor del servicio de correo electrónico.
        /// </summary>
        /// <param name="resend">El cliente de Resend.</param>
        /// <param name="configuration">La interfaz de configuración.</param>
        public EmailService(IResend resend, IConfiguration configuration)
        {
            _resend = resend;
            _configuration = configuration;
        }

        /// <summary>
        /// Envía un correo electrónico con un código de verificación.
        /// </summary>
        /// <param name="email">La dirección de correo electrónico del destinatario.</param>
        /// <param name="verificationCode">El código de verificación a enviar.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task SendVerificationCodeEmailAsync(string email, string verificationCode)
        {
            var message = new EmailMessage
            {
                To = email,
                Subject = _configuration["EmailConfiguration:VerificationSubject"] ?? throw new ArgumentNullException("El asunto del correo de verificación no puede ser nulo."),
                From = _configuration["EmailConfiguration:From"] ?? throw new ArgumentNullException("La configuración de 'From' no puede ser nula."),
                HtmlBody = $"<p style='font-size: 48px; font-weight: bold; letter-spacing: 8px;'>{verificationCode}</p>"
            };

            await _resend.EmailSendAsync(message);
        }

        /// <summary>
        /// Envía un correo electrónico de bienvenida a un nuevo usuario.
        /// </summary>
        /// <param name="email">La dirección de correo electrónico del destinatario.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task SendWelcomeEmailAsync(string email)
        {
            var message = new EmailMessage
            {
                To = email,
                Subject = _configuration["EmailConfiguration:WelcomeSubject"] ?? throw new ArgumentNullException("El asunto del correo de bienvenida no puede ser nulo."),
                From = _configuration["EmailConfiguration:From"] ?? throw new ArgumentNullException("La configuración de 'From' no puede ser nula."),
                HtmlBody = "<p style='font-size: 24px; font-weight: bold;'>¡Tu cuenta ha sido verificada exitosamente!</p>"
            };

            await _resend.EmailSendAsync(message);
        }
    }
}