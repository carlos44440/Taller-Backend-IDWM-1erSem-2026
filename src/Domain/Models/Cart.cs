namespace TiendaUCN.src.Domain.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int TotalPrice { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public List<CartItem> CartItems { get; } = [];
        public bool IsDeleted { get; set; } = false;
    }
}