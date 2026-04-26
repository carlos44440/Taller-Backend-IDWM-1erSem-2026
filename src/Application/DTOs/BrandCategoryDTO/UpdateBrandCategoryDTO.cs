using System.ComponentModel.DataAnnotations;

namespace TiendaUCN.src.Application.DTOs.BrandCategoryDTO
{
    /// <summary>
    /// DTO para la actualización de una categoría o marca.
    /// </summary>
    public class UpdateBrandCategoryDTO
    {
        /// <summary>
        /// Nombre de la categoría o marca.
        /// </summary>
        /// <remarks>
        /// Es opcional y debe tener entre 3 y 25 caracteres si se proporciona.
        /// </remarks>
        [MinLength(3, ErrorMessage = "El nombre debe tener mínimo 3 letras.")]
        [MaxLength(25, ErrorMessage = "El nombre debe tener máximo 25 letras.")]
        public string? Name { get; set; }

        /// <summary>
        /// Descripción de la categoría o marca.
        /// </summary>
        /// <remarks>
        /// Es opcional y debe tener entre 3 y 250 caracteres si se proporciona.
        /// </remarks>
        [MinLength(3, ErrorMessage = "La descripción debe tener mínimo 3 letras.")]
        [MaxLength(250, ErrorMessage = "La descripción debe tener máximo 250 letras.")]
        public string? Description { get; set; }
    }
}