namespace TiendaUCN.src.Application.DTOs.CartDTO
{
    /// <summary>
    /// DTO que representa un ítem dentro del carrito de compras.
    /// </summary>
    public class CartItemDTO
    {
        /// <summary>
        /// Identificador del producto.
        /// </summary>
        public required int ProductId { get; set; }

        /// <summary>
        /// Nombre del producto.
        /// </summary>
        public required string ProductName { get; set; }

        /// <summary>
        /// URL de la imagen del producto.
        /// </summary>
        public required string ProductImageUrl { get; set; }

        /// <summary>
        /// Precio unitario del producto.
        /// </summary>
        public required int ProductPrice { get; set; }

        /// <summary>
        /// Cantidad del producto en el carrito.
        /// </summary>
        public required int Quantity { get; set; }

        /// <summary>
        /// Precio total correspondiente a la cantidad del producto.
        /// </summary>
        public required int TotalPrice { get; set; }
    }
}