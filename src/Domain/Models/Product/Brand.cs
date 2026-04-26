namespace TiendaUCN.src.Domain.Models
{
    /// <summary>
    /// Entidad de marca.
    /// </summary>
    public class Brand
    {
        /// <summary>
        /// Identificador de la marca.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la marca.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Descripción de la marca.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Indica si la marca está eliminada.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}