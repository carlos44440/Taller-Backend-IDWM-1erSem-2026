namespace TiendaUCN.src.Application.DTOs.CartDTO
{
    /// <summary>
    /// DTO que representa un carrito de compras.
    /// </summary>
    public class CartDTO
    {
        /// <summary>
        /// Identificador del comprador asociado al carrito.
        /// </summary>
        public required string BuyerId { get; set; }

        /// <summary>
        /// Identificador del usuario asociado al carrito.
        /// </summary>
        /// <remarks>
        /// Puede ser nulo en caso de que el carrito pertenezca a un usuario no autenticado.
        /// </remarks>
        public required int? UserId { get; set; }

        /// <summary>
        /// Lista de ítems contenidos en el carrito.
        /// </summary>
        public required List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();

        /// <summary>
        /// Precio total del carrito.
        /// </summary>
        public required int TotalPrice { get; set; }
    }
}