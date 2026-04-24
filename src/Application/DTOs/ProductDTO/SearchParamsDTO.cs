using System.ComponentModel.DataAnnotations;

namespace TiendaUCN.src.Application.DTOs.ProductDTO
{
    /// <summary>
    /// DTO que representa los parámetros de búsqueda y paginación de productos.
    /// </summary>
    public class SearchParamsDTO
    {
        /// <summary>
        /// Número de página a consultar.
        /// </summary>
        /// <remarks>
        /// Es obligatorio y debe ser un número entero positivo.
        /// </remarks>
        [Required(ErrorMessage = "El número de página es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El número de página debe ser un número entero positivo.")]
        public required int PageNumber { get; set; }

        /// <summary>
        /// Cantidad de elementos por página.
        /// </summary>
        /// <remarks>
        /// Es obligatorio y debe ser un número entero positivo.
        /// </remarks>
        [Required(ErrorMessage = "El tamaño de página es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El tamaño de página debe ser un número entero positivo.")]
        public required int PageSize { get; set; }

        /// <summary>
        /// Término de búsqueda para filtrar productos.
        /// </summary>
        /// <remarks>
        /// Es opcional y debe tener entre 2 y 40 caracteres si se proporciona.
        /// </remarks>
        [MinLength(2, ErrorMessage = "El término de búsqueda debe tener al menos 2 caracteres.")]
        [MaxLength(40, ErrorMessage = "El término de búsqueda no puede exceder los 40 caracteres.")]
        public string? SearchTerm { get; set; }
    }
}