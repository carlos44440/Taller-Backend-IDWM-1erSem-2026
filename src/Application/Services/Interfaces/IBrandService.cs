using TiendaUCN.src.Application.DTOs.BrandCategoryDTO;

namespace TiendaUCN.src.Application.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de marcas.
    /// </summary>
    public interface IBrandService
    {
        /// <summary>
        /// Crea una nueva marca.
        /// </summary>
        /// <param name="brandDto">El DTO de la marca a crear.</param>
        /// <returns>Mensaje de resultado.</returns>
        Task<string> CreateBrandAsync(CreateBrandCategoryDTO brandDto);

        /// <summary>
        /// Actualiza una marca existente.
        /// </summary>
        /// <param name="brandId">El ID de la marca a actualizar.</param>
        /// <param name="brandDto">El DTO de la marca con los nuevos datos.</param>
        /// <returns>Mensaje de resultado.</returns>
        Task<string> UpdateBrandAsync(int brandId, UpdateBrandCategoryDTO brandDto);

        /// <summary>
        /// Elimina una marca por su ID.
        /// </summary>
        /// <param name="brandId">El ID de la marca a eliminar.</param>
        /// <returns>Mensaje de resultado.</returns>
        Task<string> DeleteBrandAsync(int brandId);
    }
}