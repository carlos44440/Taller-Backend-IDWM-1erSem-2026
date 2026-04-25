using Mapster;
using TiendaUCN.src.Application.DTOs.AuthDTO;
using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Application.Mappers
{
    /// <summary>
    /// Mapper para los usuarios
    /// </summary>
    public class UserMapper
    {
        /// <summary>
        /// Configura todas las mapeos para los usuarios.
        /// </summary>
        public void ConfigureAllMappings()
        {
            ConfigureAuthMappings();
        }

        /// <summary>
        /// Configura el mapeo del registro de usuarios.
        /// </summary>
        private void ConfigureAuthMappings()
        {
            TypeAdapterConfig<RegisterDTO, User>.NewConfig()
                .Map(dest => dest.PasswordHash, src => BCrypt.Net.BCrypt.HashPassword(src.Password)); // Encriptar la contraseña
        }
    }
}