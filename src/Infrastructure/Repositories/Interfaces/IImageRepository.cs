using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de imágenes.
    /// </summary>
    public interface IImageRepository
    {
        /// <summary>
        /// Crea una nueva imagen en la base de datos.
        /// </summary>
        /// <param name="image">La imagen a crear</param>
        /// <returns>true si la crea, false si no</returns>
        Task<bool?> CreateAsync(Image image);

        /// <summary>
        /// Elimina una imagen de la base de datos.
        /// </summary>
        /// <param name="publicId">El ID público de la imagen</param>
        /// <returns>true si la elimina, false si no</returns>
        Task<bool?> DeleteAsync(string publicId);
    }
}