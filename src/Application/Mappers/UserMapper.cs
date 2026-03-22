using TiendaUCN.src.Application.DTOs.AuthDTO;
using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Application.Mappers
{
    public class UserMapper
    {
        public static User ToUser(RegisterDTO registerDTO)
        {
            return new User
            {
                Name = registerDTO.Name,
                Email = registerDTO.Email,
                EmailConfirmed = false,
                Rut = registerDTO.Rut,
                PhoneNumber = registerDTO.PhoneNumber,
                BirthDate = registerDTO.BirthDate,
                Gender = registerDTO.Gender,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password), // Encriptar la contraseña
                RoleId = 2, // Asignar el rol de cliente por defecto
                IsDeleted = false
            };
        }
    }
}