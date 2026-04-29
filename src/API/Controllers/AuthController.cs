using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda_UCN_api.src.Application.DTO;
using TiendaUCN.src.Application.DTOs.AuthDTO;
using TiendaUCN.src.Application.Services.Interfaces;

namespace TiendaUCN.src.API.Controllers
{
    /// <summary>
    /// Controlador de autenticación.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Interfaz del servicio de usuario.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor del controlador de autenticación.
        /// </summary>
        /// <param name="userService">Interfaz del servicio de usuario</param>
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Endpoint para registrar un nuevo usuario.
        /// </summary>
        /// <param name="registerDTO">DTO para el registro de usuario</param>
        /// <returns>Mensaje de éxito</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var message = await _userService.RegisterAsync(registerDTO);
            return Ok(new GenericResponse<string>("Registro exitoso", message));
        }

        /// <summary>
        /// Endpoint para verificar el correo electrónico de un usuario.
        /// </summary>
        /// <param name="emailVerificationDTO">DTO para la verificación de correo electrónico</param>
        /// <returns>Mensaje de éxito</returns>
        [HttpPost("email-verification")]
        public async Task<IActionResult> EmailVerification([FromBody] EmailVerificationDTO emailVerificationDTO)
        {
            await _userService.EmailVerificationAsync(emailVerificationDTO);
            return Ok(new GenericResponse<string>("Correo electrónico verificado exitosamente", null));
        }

        /// <summary>
        /// Endpoint para iniciar sesión.
        /// </summary>
        /// <param name="loginDTO">DTO para el inicio de sesión</param>
        /// <returns>Token de autenticación</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var token = await _userService.LoginAsync(loginDTO);
            return Ok(new GenericResponse<string>("Inicio de sesión exitoso", token));
        }

        /// <summary>
        /// Endpoint para reenviar el código de verificación de correo electrónico.
        /// </summary>
        /// <param name="resendVerificationCodeDTO">DTO para el reenvío del código de verificación</param>
        /// <returns>Mensaje de éxito</returns>
        [HttpPost("resend-verification-code")]
        public async Task<IActionResult> ResendVerificationCode([FromBody] ResendVerificationCodeDTO resendVerificationCodeDTO)
        {
            var message = await _userService.ResendVerificationCodeAsync(resendVerificationCodeDTO);
            return Ok(new GenericResponse<string>("Código de verificación reenviado exitosamente", message));
        }

        /// <summary>
        /// Endpoint para cerrar sesión.
        /// </summary>
        /// <returns>Mensaje de éxito</returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var message = await _userService.LogoutAsync(token);
            return Ok(new GenericResponse<string>(message, null));
        }
    }
}