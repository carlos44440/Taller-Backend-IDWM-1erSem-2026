using System.ComponentModel.DataAnnotations;

namespace TiendaUCN.src.Application.DTOs.CartDTO
{
    /// <summary>
    /// DTO para agregar o modificar un ítem en el carrito de compras.
    /// </summary>
    public class AddChangeCartItemDTO
    {
        /// <summary>
        /// Identificador del producto a agregar o modificar en el carrito.
        /// </summary>
        /// <remarks>
        /// Es obligatorio y debe ser un número entero positivo.
        /// </remarks>
        [Required(ErrorMessage = "El ID del producto es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del producto debe ser un número positivo.")]
        public required int ProductId { get; set; }

        /// <summary>
        /// Cantidad del producto en el carrito.
        /// </summary>
        /// <remarks>
        /// Es obligatorio y debe ser un número entero positivo.
        /// </remarks>
        [Required(ErrorMessage = "La cantidad es requerida.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser un número positivo.")]
        public required int Quantity { get; set; }
    }
}