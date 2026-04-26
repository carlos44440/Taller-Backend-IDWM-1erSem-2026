using System.ComponentModel.DataAnnotations;

namespace TiendaUCN.src.Application.DTOs.ProductDTO.Admin
{
    /// <summary>
    /// DTO para la actualización de un producto.
    /// </summary>
    public class UpdateProductDTO
    {
        /// <summary>
        /// Nombre del producto.
        /// </summary>
        /// <remarks>
        /// Es opcional y debe tener entre 3 y 20 caracteres si se proporciona.
        /// </remarks>
        [StringLength(20, ErrorMessage = "El nombre no puede exceder los 20 caracteres.")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres.")]
        public string? Name { get; set; }

        /// <summary>
        /// Descripción del producto.
        /// </summary>
        /// <remarks>
        /// Es opcional y debe tener entre 10 y 100 caracteres si se proporciona.
        /// </remarks>
        [StringLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres.")]
        [MinLength(10, ErrorMessage = "La descripción debe tener al menos 10 caracteres.")]
        public string? Description { get; set; }

        /// <summary>
        /// Precio del producto.
        /// </summary>
        /// <remarks>
        /// Es opcional y debe ser un número entero positivo mayor que cero si se proporciona.
        /// </remarks>
        [Range(1, int.MaxValue, ErrorMessage = "El precio debe ser un valor entero positivo mayor que cero.")]
        public int? Price { get; set; }

        /// <summary>
        /// Stock disponible del producto.
        /// </summary>
        /// <remarks>
        /// Es opcional y debe ser un número entero positivo mayor que cero si se proporciona.
        /// </remarks>
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser un valor entero positivo mayor que cero.")]
        public int? Stock { get; set; }

        /// <summary>
        /// Nombre de la categoría del producto.
        /// </summary>
        /// <remarks>
        /// Es opcional y debe tener entre 3 y 25 caracteres si se proporciona.
        /// </remarks>
        [StringLength(25, ErrorMessage = "El nombre de la categoría no puede exceder los 25 caracteres.")]
        [MinLength(3, ErrorMessage = "El nombre de la categoría debe tener al menos 3 caracteres.")]
        public string? CategoryName { get; set; }

        /// <summary>
        /// Nombre de la marca del producto.
        /// </summary>
        /// <remarks>
        /// Es opcional y debe tener entre 3 y 25 caracteres si se proporciona.
        /// </remarks>
        [StringLength(25, ErrorMessage = "El nombre de la marca no puede exceder los 25 caracteres.")]
        [MinLength(3, ErrorMessage = "El nombre de la marca debe tener al menos 3 caracteres.")]
        public string? BrandName { get; set; }

        /// <summary>
        /// Lista de URLs de imágenes a eliminar del producto.
        /// </summary>
        public List<string>? ImagesToRemove { get; set; }

        /// <summary>
        /// Lista de nuevas imágenes a agregar al producto.
        /// </summary>
        public List<IFormFile>? ImagesToAdd { get; set; }
    }
}