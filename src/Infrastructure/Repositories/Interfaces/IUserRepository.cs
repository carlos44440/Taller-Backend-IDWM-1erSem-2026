using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de usuarios.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        /// <param name="user">El usuario a crear</param>
        Task CreateAsync(User user);

        /// <summary>
        /// Verifica si un usuario existe por su correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario</param>
        /// <returns>true si el usuario existe, false si no</returns>
        Task<bool> ExistsByEmailAsync(string email);

        /// <summary>
        /// Verifica si un usuario existe por su RUT.
        /// </summary>
        /// <param name="rut">El RUT del usuario</param>
        /// <returns>true si el usuario existe, false si no</returns>
        Task<bool> ExistsByRutAsync(string rut);

        /// <summary>
        /// Obtiene un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario</param>
        /// <returns>El usuario si existe, null si no</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Marca el correo electrónico de un usuario como verificado.
        /// </summary>
        /// <param name="id">El ID del usuario</param>
        /// <returns>true si se marca como verificado, false si no</returns>
        Task<bool> MarkEmailAsVerifiedAsync(int id);

        /// <summary>
        /// Elimina los usuarios no confirmados después de un número específico de días.
        /// </summary>
        /// <param name="daysToDeleteUnverifiedAccount">El número de días después del cual se eliminan los usuarios no confirmados</param>
        /// <returns>El número de usuarios eliminados</returns>
        Task<int> DeleteUnconfirmedUsersAsync(int daysToDeleteUnverifiedAccount);
    }
}