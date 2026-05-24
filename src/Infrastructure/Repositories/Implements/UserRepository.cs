using Microsoft.EntityFrameworkCore;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Infrastructure.Repositories.Implements
{
    /// <summary>
    /// Repositorio de usuarios.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Contexto de datos para acceder a la base de datos.
        /// </summary>
        private readonly DataContext _context;

        /// <summary>
        /// Constructor del repositorio de usuarios.
        /// </summary>
        /// <param name="context">El contexto de datos</param>
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        /// <param name="user">El usuario a crear</param>
        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Verifica si un usuario existe por su correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario</param>
        /// <returns>true si el usuario existe, false si no</returns>
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u =>
                    u.Email.ToLower() == email.ToLower() &&
                    u.IsDeleted == false);
        }

        /// <summary>
        /// Verifica si un usuario existe por su RUT.
        /// </summary>
        /// <param name="rut">El RUT del usuario</param>
        /// <returns>true si el usuario existe, false si no</returns>
        public async Task<bool> ExistsByRutAsync(string rut)
        {
            return await _context.Users
                .AnyAsync(u =>
                    u.Rut == rut &&
                    u.IsDeleted == false);
        }

        /// <summary>
        /// Obtiene un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario</param>
        /// <returns>El usuario si existe, null si no</returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            // Incluir las entidades relacionadas Role y VerificationCode al obtener el usuario por correo electrónico
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.VerificationCode)
                .FirstOrDefaultAsync(u =>
                    u.Email.ToLower() == email.ToLower()
                    && u.IsDeleted == false);
        }

        /// <summary>
        /// Marca el correo electrónico de un usuario como verificado.
        /// </summary>
        /// <param name="id">El ID del usuario</param>
        /// <returns>true si se marca como verificado, false si no</returns>
        public async Task<bool> MarkEmailAsVerifiedAsync(int id)
        {
            var result = await _context.Users.Where(u => u.Id == id).ExecuteUpdateAsync(u => u.SetProperty(x => x.EmailConfirmed, true));
            return result > 0;
        }

        /// <summary>
        /// Elimina los usuarios no confirmados después de un número específico de días.
        /// </summary>
        /// <param name="daysToDeleteUnverifiedAccount">El número de días después del cual se eliminan los usuarios no confirmados</param>
        /// <returns>El número de usuarios eliminados</returns>
        public async Task<int> DeleteUnconfirmedUsersAsync(int daysToDeleteUnverifiedAccount)
        {
            var now = DateTime.UtcNow;

            // Elimina los códigos de verificación
            // Asociados a los usuarios que cumplen con las condiciones de eliminación
            await _context.VerificationCodes
                .Where(vc =>
                    _context.Users
                        .Any(u =>
                            u.Id == vc.UserId &&
                            u.EmailConfirmed == false &&
                            u.IsDeleted == false &&
                            u.CreatedAt.AddDays(daysToDeleteUnverifiedAccount) <= now))
                .ExecuteDeleteAsync();

            // Elimina los usuarios que:
            // No han confirmado su correo
            // No han sido eliminados previamente
            // Fueron creados hace más de 'daysToDeleteUnverifiedAccount' días
            var result = await _context.Users
                .Where(x =>
                    x.EmailConfirmed == false &&
                    x.IsDeleted == false &&
                    x.CreatedAt.AddDays(daysToDeleteUnverifiedAccount) <= now)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsDeleted, true));

            return result;
        }
    }
}