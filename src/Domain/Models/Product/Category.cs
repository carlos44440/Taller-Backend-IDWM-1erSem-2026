namespace TiendaUCN.src.Domain.Models
{
    /// <summary>
    /// Entidad de categoría.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Identificador de la categoría.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la categoría.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Descripción de la categoría.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Lista de productos asociados.
        /// </summary>
        public ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Indica si la categoría está eliminada.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}