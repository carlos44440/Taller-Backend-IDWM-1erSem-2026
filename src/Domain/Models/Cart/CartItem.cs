namespace TiendaUCN.src.Domain.Models
{
    /// <summary>
    /// Entidad de ítem del carrito.
    /// </summary>
    public class CartItem
    {
        /// <summary>
        /// Identificador del ítem.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Cantidad del producto.
        /// </summary>
        public required int Quantity { get; set; }

        /// <summary>
        /// Identificador del carrito.
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// Carrito asociado al ítem.
        /// </summary>
        public Cart Cart { get; set; } = null!;

        /// <summary>
        /// Identificador del producto.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Producto asociado al ítem.
        /// </summary>
        public Product Product { get; set; } = null!;
    }
}