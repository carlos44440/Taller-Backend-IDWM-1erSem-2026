using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz del repositorio de categorías.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Verifica si una categoría existe por su nombre.
        /// </summary>
        /// <param name="name">El nombre de la categoría</param>
        /// <returns>true si existe, false si no</returns>
        Task<bool> ExistsByNameAsync(string name);

        /// <summary>
        /// Crea una nueva categoría en la base de datos.
        /// </summary>
        /// <param name="category">La categoría a crear</param>
        /// <returns>true si la crea, false si no</returns>
        Task<bool> CreateAsync(Category category);

        /// <summary>
        /// Verifica si una categoría existe por su ID.
        /// </summary>
        /// <param name="id">El ID de la categoría</param>
        /// <returns>true si existe, false si no</returns>
        Task<bool> ExistsByIdAsync(int id);

        /// <summary>
        /// Actualiza el nombre de una categoría existente.
        /// </summary>
        /// <param name="id">El ID de la categoría</param>
        /// <param name="name">El nuevo nombre de la categoría</param>
        /// <returns>true si la actualiza, false si no</returns>
        Task<bool> UpdateNameAsync(int id, string name);

        /// <summary>
        /// Actualiza la descripción de una categoría existente.
        /// </summary>
        /// <param name="id">El ID de la categoría</param>
        /// <param name="description">La nueva descripción de la categoría</param>
        /// <returns>true si la actualiza, false si no</returns>
        Task<bool> UpdateDescriptionAsync(int id, string description);

        /// <summary>
        /// Elimina una categoría existente.
        /// </summary>
        /// <param name="id">El ID de la categoría</param>
        /// <returns>true si la elimina, false si no</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Obtiene el ID de una categoría por su nombre.
        /// </summary>
        /// <param name="name">El nombre de la categoría</param>
        /// <returns>El ID de la categoría</returns>
        Task<int> GetIdByNameAsync(string name);
    }
}