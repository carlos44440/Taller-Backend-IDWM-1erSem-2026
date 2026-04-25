namespace TiendaUCN.src.Domain.Models
{
    /// <summary>
    /// Entidad de imagen.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Identificador de la imagen.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// URL de la imagen.
        /// </summary>
        public required string ImageUrl { get; set; }

        /// <summary>
        /// Identificador público de la imagen.
        /// </summary>
        public required string PublicId { get; set; }

        /// <summary>
        /// Identificador del producto.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Producto asociado a la imagen.
        /// </summary>
        public Product Product { get; set; } = null!;
    }
}