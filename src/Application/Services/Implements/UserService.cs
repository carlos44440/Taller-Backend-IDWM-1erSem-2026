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
        private readonly int _verificationCodeExpiry;

        public UserService(IEmailService emailService, IUserRepository userRepository, IConfiguration configuration)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _configuration = configuration;
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

            // Generar código de verificación
            string VerificationCode = new Random().Next(100000, 999999).ToString();
            DateTime VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(_verificationCodeExpiry);
            Log.Information($"Código de verificación generado para el usuario: {registerDTO.Email} - Código: {VerificationCode}");

            // Guardar el código de verificación y su fecha de expiración en el usuario
            bool isSaved = await _userRepository.SaveVerificationCodeAsync(userId, VerificationCode, VerificationCodeExpiry);
            if (!isSaved)
            {
                Log.Error($"Error al guardar el código de verificación para el usuario: {registerDTO.Email}");
                throw new InvalidOperationException("Error al guardar el código de verificación.");
            }

            // Enviar el código de verificación por correo electrónico
            await _emailService.SendVerificationCodeEmailAsync(registerDTO.Email, VerificationCode);
            Log.Information($"Se ha enviado un código de verificación al correo electrónico: {registerDTO.Email}");

            // Retornar mensaje de éxito
            return $"Se ha enviado un código de verificación a su correo electrónico, este código expirará en {_verificationCodeExpiry} minutos.";

        }
    }
}