using Hangfire;
using Serilog;
using TiendaUCN.src.Application.Jobs.Interfaces;
using TiendaUCN.src.Application.Services.Interfaces;

namespace TiendaUCN.src.Application.Jobs.Implements
{
    /// <summary>
    /// Trabajo para el usuario
    /// </summary>
    public class UserJob : IUserJob
    {
        /// <summary>
        /// Interfaz del servicio de usuario.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Interfaz del servicio de token.
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Constructor del trabajo de usuario.
        /// </summary>
        /// <param name="userService">Interfaz del servicio de usuario.</param>
        /// <param name="tokenService">Interfaz del servicio de token.</param>
        public UserJob(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Elimina los tokens expirados en la blacklist.
        /// </summary>
        /// <returns></returns>
        [AutomaticRetry(Attempts = 10, DelaysInSeconds = new int[] { 60, 120, 300, 600, 900 })]
        public async Task DeleteExpiredTokensInBlacklistAsync()
        {
            Log.Information("Eliminando tokens expirados en la blacklist...");
            await _tokenService.DeleteExpiredTokensInBlacklistAsync();
        }

        /// <summary>
        /// Elimina los usuarios no confirmados.
        /// </summary>
        /// <returns></returns>
        [AutomaticRetry(Attempts = 10, DelaysInSeconds = new int[] { 60, 120, 300, 600, 900 })]
        public async Task DeleteUnconfirmedUsersAsync()
        {
            // Configura el trabajo para que se reintente automáticamente en caso de fallo, con un número máximo de intentos y retrasos entre ellos
            Log.Information("Eliminando usuarios no confirmados...");
            await _userService.DeleteUnconfirmedUsersAsync();
        }
    }
}