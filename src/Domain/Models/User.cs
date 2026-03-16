namespace TiendaUCN.src.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public string? VerificationCode { get; set; }
        public DateTime VerificationCodeExpiry { get; set; }
        public required string Rut { get; set; }
        public required string PhoneNumber { get; set; }
        public required DateTime BirthDate { get; set; }
        public required string Gender { get; set; }
        public required string PasswordHash { get; set; }
        public int RoleId { get; set; } // Establece la relación con Role (Un rol puede tener muchos usuarios)
        public Role Role { get; set; } = null!;
        public Cart? Cart { get; set; }  // Referencia opcional a Cart
        public List<Order> Orders { get; set; } = new List<Order>();
        public bool IsDeleted { get; set; } = false;
    }
}