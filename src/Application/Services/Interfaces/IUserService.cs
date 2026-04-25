using TiendaUCN.src.Application.DTOs.AuthDTO;

namespace TiendaUCN.src.Application.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de usuarios.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="registerDTO">Datos del usuario a registrar.</param>
        /// <returns>Mensaje de resultado.</returns>
        Task<string> RegisterAsync(RegisterDTO registerDTO);

        /// <summary>
        /// Verifica el correo electrónico de un usuario.
        /// </summary>
        /// <param name="emailVerificationDTO">Datos de verificación.</param>
        Task EmailVerificationAsync(EmailVerificationDTO emailVerificationDTO);

        /// <summary>
        /// Inicia sesión de un usuario.
        /// </summary>
        /// <param name="loginDTO">Credenciales del usuario.</param>
        /// <returns>Token de autenticación.</returns>
        Task<string> LoginAsync(LoginDTO loginDTO);

        /// <summary>
        /// Cierra la sesión de un usuario.
        /// </summary>
        /// <param name="token">Token de autenticación.</param>
        /// <returns>Mensaje de resultado.</returns>
        Task<string> LogoutAsync(string token);

        /// <summary>
        /// Elimina usuarios no confirmados.
        /// </summary>
        /// <returns>Cantidad de usuarios eliminados.</returns>
        Task<int> DeleteUnconfirmedUsersAsync();

        /// <summary>
        /// Reenvía el código de verificación.
        /// </summary>
        /// <param name="resendVerificationCodeDTO">Datos para reenviar el código.</param>
        /// <returns>Mensaje de resultado.</returns>
        Task<string> ResendVerificationCodeAsync(ResendVerificationCodeDTO resendVerificationCodeDTO);
    }
}