namespace TiendaUCN.src.Domain.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int TotalPrice { get; set; }
        public int UserId { get; set; } // Establece la relación con User (Un usuario tiene un carrito)
        public User User { get; set; } = null!;
        public List<CartItem> CartItems { get; } = []; // Relación con CartItem (Un carrito puede tener muchos CartItems)
        public bool IsDeleted { get; set; } = false;
    }
}