using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz del repositorio de marcas.
    /// </summary>
    public interface IBrandRepository
    {
        /// <summary>
        /// Obtiene las marcas activas ordenadas por nombre.
        /// </summary>
        /// <returns>Lista de marcas activas.</returns>
        Task<List<Brand>> GetActiveAsync();

        /// <summary>
        /// Verifica si una marca existe por su nombre.
        /// </summary>
        /// <param name="name">Nombre de la marca</param>
        /// <returns>true si existe, false si no</returns>
        Task<bool> ExistsByNameAsync(string name);

        /// <summary>
        /// Crea una nueva marca en la base de datos.
        /// </summary>
        /// <param name="brand">La marca a crear</param>
        /// <returns>true si la crea, false si no</returns>
        Task<bool> CreateAsync(Brand brand);

        /// <summary>
        /// Verifica si una marca existe por su ID.
        /// </summary>
        /// <param name="id">ID de la marca</param>
        /// <returns>true si existe, false si no</returns>
        Task<bool> ExistsByIdAsync(int id);

        /// <summary>
        /// Actualiza el nombre de una marca existente.
        /// </summary>
        /// <param name="id">ID de la marca</param>
        /// <param name="name">Nuevo nombre de la marca</param>
        /// <returns>true si la actualiza, false si no</returns>
        Task<bool> UpdateNameAsync(int id, string name);

        /// <summary>
        /// Actualiza la descripción de una marca existente.
        /// </summary>
        /// <param name="id">ID de la marca</param>
        /// <param name="description">Nueva descripción de la marca</param>
        /// <returns>true si la actualiza, false si no</returns>
        Task<bool> UpdateDescriptionAsync(int id, string description);

        /// <summary>
        /// Elimina una marca de la base de datos.
        /// </summary>
        /// <param name="id">ID de la marca</param>
        /// <returns>true si la elimina, false si no</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Obtiene el ID de una marca por su nombre.
        /// </summary>
        /// <param name="name">Nombre de la marca</param>
        /// <returns>El ID de la marca si existe, 0 si no</returns>
        Task<int> GetIdByNameAsync(string name);
    }
}