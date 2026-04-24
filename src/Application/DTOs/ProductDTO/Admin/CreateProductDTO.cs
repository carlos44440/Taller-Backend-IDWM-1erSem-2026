using System.ComponentModel.DataAnnotations;

namespace TiendaUCN.src.Application.DTOs.ProductDTO
{
    /// <summary>
    /// DTO para la creación de un producto.
    /// </summary>
    public class CreateProductDTO
    {
        /// <summary>
        /// Nombre del producto.
        /// </summary>
        /// <remarks>
        /// Es obligatorio y debe tener entre 3 y 20 caracteres.
        /// </remarks>
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(20, ErrorMessage = "El nombre no puede exceder los 20 caracteres.")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres.")]
        public required string Name { get; set; }

        /// <summary>
        /// Descripción del producto.
        /// </summary>
        /// <remarks>
        /// Es obligatoria y debe tener entre 10 y 100 caracteres.
        /// </remarks>
        [Required(ErrorMessage = "La descripción del producto es obligatoria.")]
        [StringLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres.")]
        [MinLength(10, ErrorMessage = "La descripción debe tener al menos 10 caracteres.")]
        public required string Description { get; set; }

        /// <summary>
        /// Precio del producto.
        /// </summary>
        /// <remarks>
        /// Es obligatorio y debe ser un número entero positivo mayor que cero.
        /// </remarks>
        [Required(ErrorMessage = "El precio del producto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El precio debe ser un valor entero positivo mayor que cero.")]
        public required int Price { get; set; }

        /// <summary>
        /// Stock disponible del producto.
        /// </summary>
        /// <remarks>
        /// Es obligatorio y debe ser un número entero positivo mayor que cero.
        /// </remarks>
        [Required(ErrorMessage = "El stock del producto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser un valor entero positivo mayor que cero.")]
        public required int Stock { get; set; }

        /// <summary>
        /// Nombre de la categoría del producto.
        /// </summary>
        /// <remarks>
        /// Es obligatorio y debe tener entre 3 y 25 caracteres.
        /// </remarks>
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(25, ErrorMessage = "El nombre de la categoría no puede exceder los 25 caracteres.")]
        [MinLength(3, ErrorMessage = "El nombre de la categoría debe tener al menos 3 caracteres.")]
        public required string CategoryName { get; set; }

        /// <summary>
        /// Nombre de la marca del producto.
        /// </summary>
        /// <remarks>
        /// Es obligatorio y debe tener entre 3 y 25 caracteres.
        /// </remarks>
        [Required(ErrorMessage = "El nombre de la marca es obligatorio.")]
        [StringLength(25, ErrorMessage = "El nombre de la marca no puede exceder los 25 caracteres.")]
        [MinLength(3, ErrorMessage = "El nombre de la marca debe tener al menos 3 caracteres.")]
        public required string BrandName { get; set; }

        /// <summary>
        /// Lista de archivos de imágenes del producto.
        /// </summary>
        /// <remarks>
        /// Es obligatoria y debe contener al menos una imagen.
        /// </remarks>
        [Required(ErrorMessage = "Las imágenes del producto son obligatorias.")]
        [MinLength(1, ErrorMessage = "Debe proporcionar al menos una imagen para el producto.")]
        public required List<IFormFile> ImagesFiles { get; set; }
    }
}