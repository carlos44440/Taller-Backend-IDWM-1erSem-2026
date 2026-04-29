namespace TiendaUCN.src.Domain.Models
{
    /// <summary>
    /// Entidad de carrito de compras.
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// Identificador del carrito.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Precio total del carrito.
        /// </summary>
        public int TotalPrice { get; set; } = 0;

        /// <summary>
        /// Identificador del comprador.
        /// </summary>
        public string BuyerId { get; set; } = null!;

        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Usuario asociado al carrito.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Lista de ítems del carrito.
        /// </summary>
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}