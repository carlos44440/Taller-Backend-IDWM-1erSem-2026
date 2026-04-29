namespace TiendaUCN.src.Domain.Models
{
    /// <summary>
    /// Entidad de producto.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Identificador del producto.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre del producto.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Descripción del producto.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Precio del producto.
        /// </summary>
        public required int Price { get; set; }

        /// <summary>
        /// Stock disponible del producto.
        /// </summary>
        public required int Stock { get; set; }

        /// <summary>
        /// Identificador de la marca.
        /// </summary>
        public int BrandId { get; set; }

        /// <summary>
        /// Marca asociada al producto.
        /// </summary>
        public Brand Brand { get; set; } = null!;

        /// <summary>
        /// Identificador de la categoría.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Categoría asociada al producto.
        /// </summary>
        public Category Category { get; set; } = null!;

        /// <summary>
        /// Lista de imágenes del producto.
        /// </summary>
        public ICollection<Image> Images { get; set; } = new List<Image>();

        /// <summary>
        /// Lista de ítems de carrito asociados.
        /// </summary>
        public ICollection<CartItem> CartItems { get; } = new List<CartItem>();

        /// <summary>
        /// Indica si el producto está activo.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Fecha de creación del producto.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indica si el producto está eliminado.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}