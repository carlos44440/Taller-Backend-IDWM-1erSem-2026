using Serilog;
using TiendaUCN.src.Application.DTOs.AuthDTO;
using TiendaUCN.src.Application.Mappers;
using TiendaUCN.src.Application.Services.Interfaces;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Application.Services.Implements
{
    public class UserService : IUserService
    {
        // private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly int _verificationCodeExpiry;

        public UserService(IEmailService emailService, IUserRepository userRepository, IConfiguration configuration, ITokenService tokenService)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _configuration = configuration;
            _tokenService = tokenService;
            _verificationCodeExpiry = _configuration.GetValue<int>("VerificationCode:ExpirationTimeInMinutes");
        }

        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            // Validar si el usuario ya existe por nombre
            bool isRegisteredByName = await _userRepository.ExistsByNameAsync(registerDTO.Name);
            if (isRegisteredByName)
            {
                Log.Warning($"El usuario con el nombre {registerDTO.Name} ya está registrado.");
                throw new InvalidOperationException("El nombre de usuario ya está registrado.");
            }

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

            // Validar si el usuario ya existe por numero de teléfono
            bool isRegisteredByPhoneNumber = await _userRepository.ExistsByPhoneNumberAsync(registerDTO.PhoneNumber);
            if (isRegisteredByPhoneNumber)
            {
                Log.Warning($"El usuario con el numero de teléfono {registerDTO.PhoneNumber} ya está registrado.");
                throw new InvalidOperationException("El numero de teléfono ya está registrado.");
            }

            // Crear el usuario
            User user = UserMapper.ToUser(registerDTO);
            int userId = await _userRepository.CreateAsync(user);
            if (userId == 0)
            {
                Log.Error($"Error al crear el usuario con correo: {registerDTO.Email}");
                throw new InvalidOperationException("Error al crear el usuario.");
            }
            Log.Information($"Registro exitoso para el usuario: {registerDTO.Email} con Id: {userId}");

            // Generar y enviar el código de verificación
            await GenerateAndSendVerificationCodeAsync(user.Id, user.Email);

            // Retornar mensaje de éxito
            return $"Se ha enviado un código de verificación a su correo electrónico, este código expirará en {_verificationCodeExpiry} minutos.";

        }

        public async Task EmailVerificationAsync(EmailVerificationDTO emailVerificationDTO)
        {
            // Obtener el usuario por correo electrónico
            User user = await _userRepository.GetByEmailAsync(emailVerificationDTO.Email)
                ?? throw new KeyNotFoundException("No se encontró un usuario con el correo proporcionado.");

            Log.Warning($"Intento de verificación fallido: No se encontró un usuario con el correo {emailVerificationDTO.Email}");

            // Validar la expiración del código de verificación
            if (user.VerificationCodeExpiry < DateTime.UtcNow)
            {
                // Generar y enviar un nuevo código de verificación
                await GenerateAndSendVerificationCodeAsync(user.Id, user.Email);

                Log.Warning($"Intento de verificación fallido: Código de verificación expirado para el usuario {emailVerificationDTO.Email}");
                throw new InvalidOperationException("El código de verificación ha expirado, se ha enviado un nuevo código a su correo electrónico.");
            }

            // Validar el código de verificación
            if (user.VerificationCode != emailVerificationDTO.VerificationCode)
            {
                Log.Warning($"Intento de verificación fallido: Código de verificación incorrecto para el usuario {emailVerificationDTO.Email}");
                throw new InvalidOperationException("Código de verificación incorrecto.");
            }

            // Marcar el correo electrónico como verificado
            bool isVerified = await _userRepository.MarkEmailAsVerifiedAsync(user.Id);
            if (!isVerified)
            {
                Log.Error($"Error al marcar el correo electrónico como verificado para el usuario {emailVerificationDTO.Email}");
                throw new InvalidOperationException("Error al verificar el correo electrónico.");
            }

            // Enviar correo de bienvenida
            await _emailService.SendWelcomeEmailAsync(user.Email);

            Log.Information($"Correo electrónico verificado exitosamente para el usuario {emailVerificationDTO.Email}");
        }

        private async Task GenerateAndSendVerificationCodeAsync(int userId, string email)
        {
            string verificationCode = new Random().Next(100000, 999999).ToString();
            DateTime verificationCodeExpiry = DateTime.UtcNow.AddMinutes(_verificationCodeExpiry);
            Log.Information($"Código de verificación generado para el usuario: {email} - Código: {verificationCode}");

            bool isSaved = await _userRepository.SaveVerificationCodeAsync(userId, verificationCode, verificationCodeExpiry);
            if (!isSaved)
            {
                Log.Error($"Error al guardar el código de verificación para el usuario: {email}");
                throw new InvalidOperationException("Error al guardar el código de verificación.");
            }

            await _emailService.SendVerificationCodeEmailAsync(email, verificationCode);
            Log.Information($"Se ha enviado un código de verificación al correo electrónico: {email}");
        }

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
    }
}