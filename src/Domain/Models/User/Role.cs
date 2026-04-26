namespace TiendaUCN.src.Domain.Models
{
    /// <summary>
    /// Entidad de rol.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Identificador del rol.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre del rol.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Indica si el rol está eliminado.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}