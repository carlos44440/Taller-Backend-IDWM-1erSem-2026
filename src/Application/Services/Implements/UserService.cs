using Mapster;
using Serilog;
using TiendaUCN.src.Application.Abstractions;
using TiendaUCN.src.Application.DTOs.AuthDTO;
using TiendaUCN.src.Application.Services.Interfaces;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Application.Services.Implements
{
    /// <summary>
    /// Servicio de usuarios.
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Interfaz del repositorio de usuarios.
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Interfaz del repositorio de códigos de verificación.
        /// </summary>
        private readonly IVerificationCodeRepository _verificationCodeRepository;

        /// <summary>
        /// Servicio de envío de correos electrónicos.
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// Configuración de la aplicación.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Servicio de generación de tokens.
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Ejecutor de transacciones de base de datos.
        /// </summary>
        private readonly ITransactionRunner _transactionRunner;

        /// <summary>
        /// Tiempo de expiración del código de verificación.
        /// </summary>
        private readonly int _verificationCodeExpiry;

        /// <summary>
        /// Número máximo de intentos fallidos de verificación.
        /// </summary>
        private readonly int _maxFailedEmailVerificationAttempts;

        /// <summary>
        /// Tiempo de espera para reenviar el correo de verificación.
        /// </summary>
        private readonly int _waitingTimeInMinutesAfterResendEmail;

        /// <summary>
        /// Días para eliminar cuentas no verificadas.
        /// </summary>
        private readonly int _daysToDeleteUnverifiedAccount;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="UserService"/>.
        /// </summary>
        /// <param name="emailService">Servicio de envío de correos electrónicos.</param>
        /// <param name="userRepository">Interfaz del repositorio de usuarios.</param>
        /// <param name="verificationCodeRepository">Interfaz del repositorio de códigos de verificación.</param>
        /// <param name="configuration">Configuración de la aplicación.</param>
        /// <param name="tokenService">Servicio de generación de tokens.</param>
        /// <param name="transactionRunner">Ejecutor de transacciones de base de datos.</param>
        public UserService(
            IEmailService emailService,
            IUserRepository userRepository,
            IVerificationCodeRepository verificationCodeRepository,
            IConfiguration configuration,
            ITokenService tokenService,
            ITransactionRunner transactionRunner)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _verificationCodeRepository = verificationCodeRepository;
            _configuration = configuration;
            _tokenService = tokenService;
            _transactionRunner = transactionRunner;
            _verificationCodeExpiry = _configuration.GetValue<int>("VerificationCode:ExpirationTimeInMinutes");
            _maxFailedEmailVerificationAttempts = _configuration.GetValue<int>("VerificationCode:MaxFailedAttempts");
            _waitingTimeInMinutesAfterResendEmail = _configuration.GetValue<int>("VerificationCode:WaitingTimeInMinutesAfterResendEmail");
            _daysToDeleteUnverifiedAccount = _configuration.GetValue<int>("Jobs:DaysToDeleteUnverifiedAccount");
        }

        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="registerDTO">Datos del usuario a registrar.</param>
        /// <returns>Mensaje de resultado.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            // Validar si el usuario ya existe por correo
            bool isRegisteredByEmail = await _userRepository.ExistsByEmailAsync(registerDTO.Email);
            if (isRegisteredByEmail)
            {
                Log.Warning($"El usuario con el correo {registerDTO.Email} ya está registrado.");
                throw new InvalidOperationException("El correo ya está registrado.");
            }

            // Validar si el usuario ya existe por RUT
            bool isRegisteredByRut = await _userRepository.ExistsByRutAsync(registerDTO.Rut);
            if (isRegisteredByRut)
            {
                Log.Warning($"El usuario con el RUT {registerDTO.Rut} ya está registrado.");
                throw new InvalidOperationException("El RUT ya está registrado.");
            }

            // Crear el usuario
            var user = registerDTO.Adapt<User>();

            // Generar código de verificación y su fecha de expiración
            var (verificationCode, verificationCodeExpiry) = await GenerateCodeAndExpiryAsync();

            await _transactionRunner.ExecuteAsync(async () =>
            {
                await _userRepository.CreateAsync(user);

                // Crear la entidad de VerificationCode
                var verificationCodeEntity = new VerificationCode
                {
                    Code = verificationCode,
                    Expiry = verificationCodeExpiry,
                    UserId = user.Id
                };

                // Guardar el código de verificación en la base de datos
                var createdVerificationCode = await _verificationCodeRepository.CreateAsync(verificationCodeEntity);
                Log.Information($"Código de verificación generado para el usuario: {user.Email} - Código: {createdVerificationCode.Code}");

                await _emailService.SendVerificationCodeEmailAsync(user.Email, createdVerificationCode.Code);
            });

            Log.Information($"Registro exitoso para el usuario: {user.Email} con Id: {user.Id}");
            Log.Information($"Se ha enviado un código de verificación al correo electrónico: {user.Email}");

            // Retornar mensaje de éxito
            return $"Se ha enviado un código de verificación a su correo electrónico, este código expirará en {_verificationCodeExpiry} minutos, debe verificar su cuenta antes de que pasen {_daysToDeleteUnverifiedAccount} días o será eliminada.";
        }

        /// <summary>
        /// Verifica el correo electrónico de un usuario.
        /// </summary>
        /// <param name="emailVerificationDTO">Datos de verificación.</param>
        /// <returns>Mensaje de resultado.</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task EmailVerificationAsync(EmailVerificationDTO emailVerificationDTO)
        {
            // Obtener el usuario por correo electrónico
            User? user = await _userRepository.GetByEmailAsync(emailVerificationDTO.Email);
            if (user == null)
            {
                Log.Warning($"Intento de verificación fallido: No se encontró un usuario con el correo {emailVerificationDTO.Email}");
                throw new KeyNotFoundException("No se encontró un usuario con el correo proporcionado.");
            }

            // Validar si el correo electrónico ya está verificado
            if (user.EmailConfirmed)
            {
                Log.Warning($"Intento de verificación fallido: El correo electrónico ya está verificado para el usuario {user.Email}");
                throw new InvalidOperationException("El correo electrónico ya está verificado.");
            }

            // Verificar el número de intentos fallidos de verificación de correo para el usuario
            if (user.VerificationCode.FailedAttempts >= _maxFailedEmailVerificationAttempts)
            {
                Log.Warning($"Intento de verificación fallido: Demasiados intentos fallidos para el usuario {user.Email}");
                throw new InvalidOperationException("Demasiados intentos fallidos. Tu cuenta sera eliminada.");
            }

            // Validar el código de verificación, solo si el número de intentos fallidos es menor al máximo permitido
            if (user.VerificationCode.Code != emailVerificationDTO.VerificationCode)
            {
                // Incrementar el contador de intentos fallidos de verificación de correo para el usuario
                var isUpdated = await _verificationCodeRepository.UpdateFailedAttemptsAsync(user.VerificationCode.Id);
                if (!isUpdated)
                {
                    Log.Error($"Error al actualizar el contador de intentos fallidos de verificación de correo para el usuario {user.Email}");
                    throw new InvalidOperationException("Error al actualizar el contador de intentos fallidos.");
                }

                // Calcular los intentos disponibles después de actualizar el contador de intentos fallidos
                var failedAttemptsAfterUpdate = user.VerificationCode.FailedAttempts + 1;
                var availableAttempts = _maxFailedEmailVerificationAttempts - failedAttemptsAfterUpdate;

                Log.Warning($"Intento de verificación fallido: Código de verificación incorrecto para el usuario {user.Email}");
                throw new InvalidOperationException("Código de verificación incorrecto, tienes " + availableAttempts + " intentos disponibles antes de que tu cuenta sea eliminada.");
            }

            // Validar la expiración del código de verificación, solo si el código es correcto
            // Evitar que se genere un nuevo código si el usuario aún no ha ingresado el código correcto
            if (user.VerificationCode.Expiry < DateTime.UtcNow)
            {
                Log.Warning($"Intento de verificación fallido: Código de verificación expirado para el usuario {user.Email}");

                // Generar y enviar un nuevo código de verificación
                var (verificationCode, verificationCodeExpiry) = await GenerateCodeAndExpiryAsync();

                await _transactionRunner.ExecuteAsync(async () =>
                {
                    var isUpdated = await _verificationCodeRepository.UpdateAsync(user.VerificationCode.Id, verificationCode, verificationCodeExpiry);
                    if (!isUpdated)
                    {
                        Log.Error($"Error al actualizar el código de verificación para el usuario {user.Email}");
                        throw new InvalidOperationException("Error al actualizar el código de verificación.");
                    }

                    await _emailService.SendVerificationCodeEmailAsync(user.Email, verificationCode);
                });

                Log.Information($"Se ha enviado un código de verificación al correo electrónico: {user.Email}");

                throw new InvalidOperationException("El código de verificación ha expirado, se ha enviado un nuevo código a su correo electrónico.");
            }

            await _transactionRunner.ExecuteAsync(async () =>
            {
                // Marcar el correo electrónico como verificado
                bool isVerified = await _userRepository.MarkEmailAsVerifiedAsync(user.Id);
                if (!isVerified)
                {
                    Log.Error($"Error al marcar el correo electrónico como verificado para el usuario {user.Email}");
                    throw new InvalidOperationException("Error al verificar el correo electrónico.");
                }

                // Enviar correo de bienvenida
                await _emailService.SendWelcomeEmailAsync(user.Email);
            });

            Log.Information($"Correo electrónico verificado exitosamente para el usuario {user.Email}");
        }

        /// <summary>
        /// Genera un código de verificación y su fecha de expiración.
        /// </summary>
        /// <returns>Tupla con el código de verificación y su fecha de expiración.</returns>
        private async Task<(string, DateTime)> GenerateCodeAndExpiryAsync()
        {
            // Generar un codigo de verificación y su fecha de expiración
            string verificationCode = new Random().Next(100000, 999999).ToString();
            DateTime verificationCodeExpiry = DateTime.UtcNow.AddMinutes(_verificationCodeExpiry);

            return await Task.FromResult((verificationCode, verificationCodeExpiry));
        }

        /// <summary>
        /// Inicia sesión para un usuario autenticado.
        /// </summary>
        /// <param name="loginDTO">Credenciales del usuario.</param>
        /// <returns>Token de autenticación.</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<string> LoginAsync(LoginDTO loginDTO)
        {
            // Obtener el usuario por correo electrónico
            User user = await _userRepository.GetByEmailAsync(loginDTO.Email)
                ?? throw new KeyNotFoundException("Credenciales inválidas.");

            // Validar la contraseña
            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
            {
                Log.Warning($"Intento de inicio de sesión fallido: Contraseña incorrecta para el usuario {loginDTO.Email}");
                throw new InvalidOperationException("Credenciales inválidas.");
            }

            // Validar si el correo electrónico está verificado
            if (!user.EmailConfirmed)
            {
                Log.Warning($"Intento de inicio de sesión fallido: Correo electrónico no verificado para el usuario {loginDTO.Email}");
                throw new InvalidOperationException("Credenciales inválidas. Por favor, verifica tu correo electrónico antes de iniciar sesión.");
            }

            // Generar token JWT
            string token = _tokenService.GenerateToken(user, user.Role.Name);
            Log.Information($"Token JWT generado para el usuario: {token}");

            Log.Information($"Inicio de sesión exitoso para el usuario: {loginDTO.Email}");
            return token;
        }

        /// <summary>
        /// Cierra la sesión de un usuario.
        /// </summary>
        /// <param name="token">Token de autenticación.</param>
        /// <returns>Mensaje de resultado.</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<string> LogoutAsync(string token)
        {
            // Validar que se haya proporcionado un token
            if (string.IsNullOrEmpty(token))
            {
                Log.Warning("Intento de logout fallido: Token no proporcionado");
                throw new ArgumentException("Token es requerido para el logout.");
            }

            // Agregar el token a la blacklist
            await _tokenService.AddToBlacklistAsync(token);

            Log.Information($"Token JWT agregado a la blacklist: {token}");
            return "Logout exitoso.";
        }

        /// <summary>
        /// Elimina los usuarios no confirmados.
        /// </summary>
        /// <returns>Cantidad de usuarios eliminados.</returns>
        public async Task<int> DeleteUnconfirmedUsersAsync()
        {
            int deletedUsers = await _userRepository.DeleteUnconfirmedUsersAsync(_daysToDeleteUnverifiedAccount);
            Log.Information($"Usuarios no confirmados eliminados exitosamente. Cantidad: {deletedUsers}");
            return deletedUsers;
        }

        /// <summary>
        /// Reenvía el código de verificación a un usuario que no ha verificado su correo electrónico.
        /// </summary>
        /// <param name="resendVerificationCodeDTO">DTO con los datos para reenviar el código de verificación.</param>
        /// <returns>Mensaje de resultado.</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<string> ResendVerificationCodeAsync(ResendVerificationCodeDTO resendVerificationCodeDTO)
        {
            // Obtener el usuario por correo electrónico
            User? user = await _userRepository.GetByEmailAsync(resendVerificationCodeDTO.Email)
                ?? throw new KeyNotFoundException($"No se encontró un usuario con el correo proporcionado: {resendVerificationCodeDTO.Email}");

            // Validar si el correo electrónico ya está verificado
            if (user.EmailConfirmed)
            {
                Log.Warning($"Intento de reenvío de código de verificación fallido: El correo electrónico ya está verificado para el usuario {user.Email}");
                throw new InvalidOperationException($"El correo electrónico {user.Email} ya está verificado.");
            }

            // Validar si el usuario puede solicitar un nuevo código de verificación
            if (user.VerificationCode.DateToResend > DateTime.UtcNow)
            {
                var minutesToWait = (user.VerificationCode.DateToResend - DateTime.UtcNow).TotalMinutes;
                Log.Warning($"Intento de reenvío de código de verificación fallido: El usuario {user.Email} debe esperar {minutesToWait} minutos antes de solicitar un nuevo código");
                throw new InvalidOperationException($"Debes esperar {Math.Ceiling(minutesToWait)} minutos antes de solicitar un nuevo código de verificación.");
            }

            // Generar y enviar un nuevo código de verificación
            var (verificationCode, verificationCodeExpiry) = await GenerateCodeAndExpiryAsync();
            var newDateToResend = DateTime.UtcNow.AddMinutes(_waitingTimeInMinutesAfterResendEmail);

            await _transactionRunner.ExecuteAsync(async () =>
            {
                var isUpdated = await _verificationCodeRepository.UpdateAsync(user.VerificationCode.Id, verificationCode, verificationCodeExpiry);
                if (!isUpdated)
                {
                    Log.Error($"Error al actualizar el código de verificación para el usuario {user.Email}");
                    throw new InvalidOperationException("Error al actualizar el código de verificación.");
                }

                var isDateToResendUpdated = await _verificationCodeRepository.UpdateDateToResendAsync(user.VerificationCode.Id, newDateToResend);
                if (!isDateToResendUpdated)
                {
                    Log.Error($"Error al actualizar la fecha para reenviar un nuevo código de verificación para el usuario {user.Email}");
                    throw new InvalidOperationException("Error al actualizar la fecha para reenviar un nuevo código de verificación.");
                }

                await _emailService.SendVerificationCodeEmailAsync(user.Email, verificationCode);
            });

            return $"El código expirará en {_verificationCodeExpiry} minutos. Puedes solicitar un nuevo código después de {_waitingTimeInMinutesAfterResendEmail} minutos.";
        }
    }
}