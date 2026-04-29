
using TiendaUCN.src.Application.DTOs.BrandCategoryDTO;

namespace TiendaUCN.src.Application.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de categorías.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Crea una nueva categoría.
        /// </summary>
        /// <param name="createCategoryDTO">El DTO para crear la categoría.</param>
        /// <returns>Mensaje de resultado.</returns>
        Task<string> CreateCategoryAsync(CreateBrandCategoryDTO createCategoryDTO);

        /// <summary>
        /// Actualiza una categoría por su ID.
        /// </summary>
        /// <param name="id">El ID de la categoría a actualizar.</param>
        /// <param name="updateCategoryDTO">El DTO para actualizar la categoría.</param>
        /// <returns>Mensaje de resultado.</returns>
        Task<string> UpdateCategoryAsync(int id, UpdateBrandCategoryDTO updateCategoryDTO);

        /// <summary>
        /// Elimina una categoría por su ID.
        /// </summary>
        /// <param name="id">El ID de la categoría a eliminar.</param>
        /// <returns>Mensaje de resultado.</returns>
        Task<string> DeleteCategoryAsync(int id);
    }
}