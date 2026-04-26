namespace TiendaUCN.src.Domain.Models
{
    /// <summary>
    /// Entidad de usuario.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Indica si el correo está confirmado.
        /// </summary>
        public bool EmailConfirmed { get; set; } = false;

        /// <summary>
        /// RUT del usuario.
        /// </summary>
        public required string Rut { get; set; }

        /// <summary>
        /// Número de teléfono del usuario.
        /// </summary>
        public required string PhoneNumber { get; set; }

        /// <summary>
        /// Fecha de nacimiento del usuario.
        /// </summary>
        public required DateTime BirthDate { get; set; }

        /// <summary>
        /// Género del usuario.
        /// </summary>
        public required string Gender { get; set; }

        /// <summary>
        /// Hash de la contraseña.
        /// </summary>
        public required string PasswordHash { get; set; }

        /// <summary>
        /// Identificador del rol.
        /// </summary>
        public int RoleId { get; set; } = 2;

        /// <summary>
        /// Rol asociado al usuario.
        /// </summary>
        public Role Role { get; set; } = null!;

        /// <summary>
        /// Código de verificación asociado.
        /// </summary>
        public VerificationCode VerificationCode { get; set; } = null!;

        /// <summary>
        /// Carrito asociado al usuario.
        /// </summary>
        public Cart Cart { get; set; } = null!;

        /// <summary>
        /// Lista de pedidos del usuario.
        /// </summary>
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        /// <summary>
        /// Fecha de creación del usuario.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indica si el usuario está eliminado.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}